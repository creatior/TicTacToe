% База данных для игры крестики-нолики 4x4 с ИИ на Prolog
% Состояние доски: список из 16 элементов (0 - пусто, 1 - крестик, 2 - нолик)
% Разные уровни сложности для компьютера

:- use_module(library(random)).

%--------------------------------------------------
% 1) entry_point_primitive(+Board)
% "Глупый" бот без блокировки: выигрыша нет -> случайный ход
entry_point_primitive(Board, ResultBoard) :-
    Player = 2,
    ( find_winning_move(Board, Player, NewBoard) -> true
    ; random_move(Board, Player, NewBoard)
    ),
    ResultBoard = NewBoard, !.

% 2) entry_point_easy(+Board)
% Как primitive, но с проверкой на блокировку победы оппонента
entry_point_easy(Board, ResultBoard) :-
    Player = 2,
    ( find_winning_move(Board, Player, NewBoard) -> true
    ; Opponent = 1,
      ( find_blocking_move(Board, Opponent, BlockPos) ->
           set_cell(Board, BlockPos, Player, NewBoard)
      ; random_move(Board, Player, NewBoard)
      )
    ),
    ResultBoard = NewBoard,!.

% 3) entry_point_medium(+Board)
% Как hard, но без приоритета углов: если нет выигрыша, нет угрозы, и центры заняты -> случайный ход
entry_point_medium(Board, ResultBoard) :-
    Player = 2,
    ( find_winning_move(Board, Player, NewBoard) -> true
    ; Opponent = 1,
      ( find_blocking_move(Board, Opponent, BlockPos) ->
          set_cell(Board, BlockPos, Player, NewBoard)
      ; % эвристика: центр или случайно
        center_positions(Centers),
        ( try_positions(Board, Player, Centers, NewBoard) -> true
        ; random_move(Board, Player, NewBoard)
        )
      )
    ),
    ResultBoard = NewBoard, !.

% 4) entry_point_hard(+Board)
% Полная логика: выигрыш -> блокировка -> центр -> угол -> любой
entry_point_hard(Board, ResultBoard) :-
    next_move(Board, NewBoard),
    ResultBoard = NewBoard,!.

%--------------------------------------------------
% next_move(+Board, -NewBoard)
% Логика хода компьютера (Player = 2)
next_move(Board, NewBoard) :-
    Player = 2,
    ( % 1) Если сам могу выиграть — выигрывающий ход
      find_winning_move(Board, Player, NewBoard)
    ; % 2) Иначе: если угроза — блок
      Opponent = 1,
      ( find_blocking_move(Board, Opponent, BlockPos) ->
          set_cell(Board, BlockPos, Player, NewBoard)
      ; % 3) Эвристика: центр
        best_heuristic_move(Board, Player, NewBoard)
      )
    ).

%--------------------------------------------------
% Поиск своего выигрышного хода (четыре в ряд)
find_winning_move(Board, Player, NewBoard) :-
    empty_positions(Board, EmptyPosList),
    member(Pos, EmptyPosList),
    set_cell(Board, Pos, Player, TempBoard),
    winner(TempBoard, Player),
    NewBoard = TempBoard,
    !.

%--------------------------------------------------
% Поиск хода-блока: три противника в ряд + одно пустое
find_blocking_move(Board, Opponent, BlockPos) :-
    winning_lines(Lines),
    member(Line, Lines),
    line_threat(Board, Line, Opponent, BlockPos),
    !.

% line_threat(+Board, +Line, +Player, -EmptyPos)
line_threat(Board, Line, Player, EmptyPos) :-
    findall(P, (member(P, Line), cell(Board, P, Player)), Ps), length(Ps, 3),
    findall(E, (member(E, Line), cell(Board, E, 0)), [EmptyPos]).

%--------------------------------------------------
% Эвристический выбор хода: центр → угол → любой
best_heuristic_move(Board, Player, NewBoard) :-
    center_positions(Centers),
    try_positions(Board, Player, Centers, NewBoard), !.
best_heuristic_move(Board, Player, NewBoard) :-
    corner_positions(Corners),
    try_positions(Board, Player, Corners, NewBoard), !.
best_heuristic_move(Board, Player, NewBoard) :-
    empty_positions(Board, [Pos|_]),
    set_cell(Board, Pos, Player, NewBoard).

% try_positions(+Board, +Player, +PositionsList, -NewBoard)
try_positions(Board, Player, [Pos|_], NewBoard) :-
    cell(Board, Pos, 0), !,
    set_cell(Board, Pos, Player, NewBoard).
try_positions(Board, Player, [_|Rest], NewBoard) :-
    try_positions(Board, Player, Rest, NewBoard).
try_positions(_, _, [], _) :- fail.

%--------------------------------------------------
% Случайный ход
random_move(Board, Player, NewBoard) :-
    empty_positions(Board, Empty),
    random_member(Pos, Empty),
    set_cell(Board, Pos, Player, NewBoard).

%--------------------------------------------------
% Утилиты по работе с доской
empty_positions(Board, Positions) :-
    findall(Pos, (between(1,16,Pos), cell(Board, Pos, 0)), Positions).
first_empty_position(Board, Pos) :- empty_positions(Board, [Pos|_]).
cell(Board, Pos, Value) :- nth1(Pos, Board, Value).
set_cell(OldBoard, Pos, Value, NewBoard) :- set_nth(OldBoard, Pos, Value, NewBoard).
set_nth([_|T], 1, Value, [Value|T]).
set_nth([H|T], I, Value, [H|R]) :- I>1, NI is I-1, set_nth(T, NI, Value, R).

%--------------------------------------------------
% Проверка победы (четыре в ряд)
winner(Board, Player) :- winning_lines(Lines), member(L, Lines), line_all_player(Board, L, Player), !.
line_all_player(Board, [A,B,C,D], P) :- cell(Board,A,P), cell(Board,B,P), cell(Board,C,P), cell(Board,D,P).

winning_lines([
    [1,2,3,4], [5,6,7,8], [9,10,11,12], [13,14,15,16],
    [1,5,9,13], [2,6,10,14], [3,7,11,15], [4,8,12,16],
    [1,6,11,16], [4,7,10,13]
]).

% Позиции для эвристики
center_positions([6,7,10,11]).
corner_positions([1,4,13,16]).

% is_draw(+Board)
% Проверяет, является ли текущее состояние доски ничьей (все клетки заполнены и нет победителя)
is_draw(Board) :-
    \+ member(0, Board),       % Нет пустых клеток
    \+ winner(Board, 1),       % Нет победителя крестиков
    \+ winner(Board, 2).       % Нет победителя ноликов