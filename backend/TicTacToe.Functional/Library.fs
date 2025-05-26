namespace TicTacToe.Functional


open System
open TicTacToe.DataAccess.Entities;
open TicTacToe.DataAccess;
open TicTacToe.Application.Services;
open TicTacToe.Core.Models;

module StatsQueries =

    /// Глобальная статистика всех пользователей: (wins, losses, draws)
    let getGlobalStats (context: TicTacToeDbContext) : int * int * int =
        let grouped =
            context.Set<GameEntity>()
            |> Seq.choose (fun g -> Option.ofNullable g.Result)
            |> Seq.countBy id
            |> Map.ofSeq

        let getCount key = Map.tryFind key grouped |> Option.defaultValue 0

        let wins = getCount 1u
        let losses = getCount 2u
        let draws = getCount 3u

        (wins, losses, draws)

    /// Количество игр по уровням сложности (Difficulty -> count)
    let getGamesByDifficulty (context: TicTacToeDbContext) : seq<uint * int> =
        context.Set<GameEntity>()
        |> Seq.choose (fun g -> Option.ofNullable g.Difficulty)
        |> Seq.groupBy id
        |> Seq.map (fun (diff, games) -> diff, Seq.length games)

    /// Максимальное количество побед подряд для пользователя (через match)
    let getMaxWinStreak (context: TicTacToeDbContext) (userId: Guid) : int =
        context.Set<GameEntity>()
        |> Seq.filter (fun g -> Option.ofNullable g.UserId = Some userId)
        |> Seq.choose (fun g ->
            match Option.ofNullable g.Result with
            | Some result -> Some (g.Date, result)  // ← тут просто g.Date
            | None -> None)
        |> Seq.sortBy fst
        |> Seq.map snd
        |> Seq.fold (fun (maxStreak, currentStreak) result ->
            match result with
            | 1u ->
                let newStreak = currentStreak + 1
                (max maxStreak newStreak, newStreak)
            | _ ->
                (maxStreak, 0)
        ) (0, 0)
        |> fst

    
    /// Статистика результатов пользователя по уровням сложности (Difficulty -> wins/losses/draws)
    let getUserStatsByDifficulty (context: TicTacToeDbContext) (userId: Guid) : seq<uint * int * int * int> =
        
        let difficulties = [1u; 2u; 3u; 4u]

        let userGames =
            context.Set<GameEntity>()
            |> Seq.filter (fun g ->
                match Option.ofNullable g.UserId with
                | Some uid when uid = userId -> true
                | _ -> false)
            |> Seq.choose (fun g ->
                match Option.ofNullable g.Difficulty, Option.ofNullable g.Result with
                | Some diff, Some res -> Some (diff, res)
                | _ -> None)

        
        let grouped =
            userGames
            |> Seq.groupBy fst
            |> Map.ofSeq

        difficulties
        |> Seq.map (fun diff ->
            match Map.tryFind diff grouped with
            | Some games ->
                let results = games |> Seq.map snd
                let wins   = results |> Seq.filter ((=) 1u) |> Seq.length
                let losses = results |> Seq.filter ((=) 2u) |> Seq.length
                let draws  = results |> Seq.filter ((=) 3u) |> Seq.length
                diff, wins, losses, draws
            | None ->
                diff, 0, 0, 0
        )
    
        /// Статистика по последним 10 играм пользователя:
    let getRecentStats (context: TicTacToeDbContext) (userId: Guid) : int * int * int * int * float =
        // Берём завершённые игры пользователя с результатом
        let recentGames =
            context.Set<GameEntity>()
            |> Seq.filter (fun g -> Option.ofNullable g.UserId = Some userId && Option.ofNullable g.Finished = Some true)
            |> Seq.choose (fun g ->
                match Option.ofNullable g.Result with
                | Some r -> Some (g.Date, r)
                | None -> None)
            |> Seq.sortByDescending fst   // самые свежие игры первыми
            |> Seq.map snd                 // оставляем только результаты
            |> Seq.truncate 10             // берём не более 10
            |> Seq.toList

        let total     = List.length recentGames
        let wins      = recentGames |> List.filter ((=) 1u) |> List.length
        let losses    = recentGames |> List.filter ((=) 2u) |> List.length
        let draws     = recentGames |> List.filter ((=) 3u) |> List.length
        let winPctRaw = if total = 0 then 0.0 else float wins / float total * 100.0
        let winPct    = System.Math.Round(winPctRaw, 3)
        (total, wins, losses, draws, winPct)