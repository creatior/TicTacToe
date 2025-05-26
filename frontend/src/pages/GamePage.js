import React, { useContext, useState, useEffect } from 'react';
import { Container, Row, Col, Button, Dropdown, Card, Modal, Image } from 'react-bootstrap';
import Auth from '../components/Auth';
import { Context } from '..';
import { update, create, move, fetchUnfinished } from '../services/gameAPI';
import { getGlobal, getDifficulty, getUserMaxWinstreak, getUserDifficulty, getUserRecent } from '../services/statsAPI';
import primateImg from '../assets/primate.jpg';
import easyImg from '../assets/easy.jpg';
import mediumImg from '../assets/medium.jpg';
import hardImg from '../assets/hard.jpg';

function useStats(userId, showStats) {
  const [stats, setStats] = useState({
    global: null,
    difficulty: null,
    userMaxStreak: null,
    userByDiff: null,
    userRecent: null,
  });

  useEffect(() => {
    if (!showStats) return;
    async function fetchAll() {
      try {
        const [global, diff, maxStreak, byDiff, recent] = await Promise.all([
          getGlobal(),
          getDifficulty(),
          getUserMaxWinstreak(userId),
          getUserDifficulty(userId),
          getUserRecent(userId),
        ]);
        setStats({
          global,
          difficulty: diff,
          userMaxStreak: maxStreak.maxWinStreak,
          userByDiff: byDiff,
          userRecent: recent,
        });
      } catch (e) {
        console.error('Failed to fetch stats', e);
      }
    }
    fetchAll();
  }, [userId, showStats]);

  return stats;
}

const EMPTY_BOARD = Array(16).fill(0);
const DIFFICULTY_LEVELS = {
  1: { label: 'Primate', img: primateImg },
  2: { label: 'Easy', img: easyImg },
  3: { label: 'Medium', img: mediumImg },
  4: { label: 'Hard', img: hardImg },
};
const END_STATUSES = ['PlayerWin', 'BotWin', 'Draw'];
const RESULT_CODES = {
  PlayerWin: 1,
  BotWin: 2,
  Draw: 3,
};

const GamePage = () => {
  const [showAuth, setShowAuth] = useState(false);
  const [difficulty, setDifficulty] = useState('1');
  const [board, setBoard] = useState(EMPTY_BOARD);
  const [isLocked, setIsLocked] = useState(false);
  const [gameStatus, setGameStatus] = useState(null);
  const [showStats, setShowStats] = useState(false);
  const [gameId, setGameId] = useState(null);
  const [showContinueModal, setShowContinueModal] = useState(false);
  const [unfinished, setUnfinished] = useState(null);
  const { user } = useContext(Context);

  useEffect(() => {
    if (!user.isAuth) setShowAuth(true);
  }, [user.isAuth]);

  const storedUserId = localStorage.getItem('userId');
  const effectiveUserId = storedUserId || user.id;
  
  useEffect(() => {
    setBoard(EMPTY_BOARD);
    setGameId(null);
    setGameStatus(null);
    setDifficulty('1');
    setIsLocked(false);
  }, [user.isAuth]);

  useEffect(() => {
    if (user.isAuth) {
      (async () => {
        try {
          const data = await fetchUnfinished(storedUserId);
          if (data && data.id) {
            setUnfinished(data);
            setShowContinueModal(true);
          }
        } catch (e) {
          console.error('Fetch unfinished failed', e);
        }
      })();
    }
  }, [user.isAuth]);

  // статистика через хук
  const stats = useStats(effectiveUserId, showStats);

  const startNewGame = () => {
    setBoard(EMPTY_BOARD);
    setGameStatus(null);
    setIsLocked(false);
    setGameId(null);
    setDifficulty('1');
  };

  const continueGame = () => setGameStatus(null);

  const handleCellClick = async (index) => {
    if (board[index] !== 0 || isLocked || gameStatus) return;
    const playerBoard = [...board]; playerBoard[index] = 1;
    setBoard(playerBoard); setIsLocked(true);
    try {
      let currentId = gameId;
      if (!currentId) {
        const stateStr = playerBoard.join('');
        const created = await create(stateStr, difficulty, false, effectiveUserId, null);
        currentId = created; setGameId(currentId);
      }
      const { newBoard: updatedBoard, gameStatus: status } = await move(playerBoard, difficulty);
      setBoard(updatedBoard);
      const finished = END_STATUSES.includes(status);
      const result = finished ? RESULT_CODES[status] : null;
      const stateStr = updatedBoard.join('');
      await update(currentId, stateStr, difficulty, finished, effectiveUserId, result);
      if (finished) setGameStatus(status); else setGameStatus(null);
    } catch (error) {
      console.error('Error saving/updating', error);
    } finally { setIsLocked(false); }
  };

  const handleContinue = () => {
    const { state, difficulty: diff, id } = unfinished;
    // загружаем доску
    setBoard(state.split('').map(c => parseInt(c, 10)));
    setDifficulty(String(diff));
    setGameId(id);
    setShowContinueModal(false);
  };

  const handleDiscard = async () => {
    try {
      // помечаем старую игру завершенной
      await update(unfinished.id, unfinished.state, unfinished.difficulty, true, effectiveUserId, null);
    } catch (e) {
      console.error('Discard failed', e);
    }
    setShowContinueModal(false);
    startNewGame();
  };

  const currentImg = DIFFICULTY_LEVELS[difficulty].img;
  const currentLabel = DIFFICULTY_LEVELS[difficulty].label;

  return (
    <Container className="mt-4 d-flex justify-content-center align-items-center" style={{ minHeight: '100vh' }}>
      <Card className="p-5 w-100" style={{ maxWidth: '1400px' }}>
        <Row className="mb-3">
          <Col><h1>Крестики-нолики 4x4</h1></Col>
        </Row>
        <Row>
          <Col xs={12} md={3} className="d-flex flex-column align-items-center mb-3">
            <div style={{ width: '100%', maxWidth: '250px', aspectRatio: '1 / 1', overflow: 'hidden' }}>
              <Image
                src={currentImg}
                alt="Game Icon"
                fluid
                style={{ width: '100%', height: '100%', objectFit: 'cover' }}
              />
            </div>
            <h3 className="mt-2">Ваш оппонент: {currentLabel}</h3>
          </Col>

          <Col xs={12} md={6} className="mb-4">
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4, minmax(100px, 1fr))', gap: '2px' }}>
              {board.map((cell, i) => (
                <Button
                  key={i}
                  variant="outline-dark"
                  style={{ height: '100px', fontSize: '1.5rem' }}
                  onClick={() => handleCellClick(i)}
                  disabled={isLocked || gameStatus}
                >
                  {cell === 1 ? 'X' : cell === 2 ? 'O' : ''}
                </Button>
              ))}
            </div>
          </Col>

          <Col xs={12} md={3} className="d-flex flex-column justify-content-center">
            <div className="mb-4">
              <h4>Уровень сложности</h4>
              <Dropdown onSelect={(key) => setDifficulty(key)}>
                <Dropdown.Toggle
                  variant="secondary"
                  style={{ width: '100%' }}
                  disabled={!!gameId}
                >
                  {currentLabel}
                </Dropdown.Toggle>
                <Dropdown.Menu>
                  {Object.entries(DIFFICULTY_LEVELS).map(([key, { label }]) => (
                    <Dropdown.Item key={key} eventKey={key}>{label}</Dropdown.Item>
                  ))}
                </Dropdown.Menu>
              </Dropdown>
            </div>

            <div className="mb-4 d-grid gap-2">
              <Button onClick={startNewGame}>Новая игра</Button>
              <Button onClick={() => {
                if (user.isAuth) {
                  user.setIsAuth(false);
                  user.setUser({});
                }
                setShowAuth(true);
              }}>
                {user.isAuth ? 'Сменить аккаунт' : 'Войти'}
              </Button>
              <Button onClick={() => setShowStats(true)}>Статистика</Button>
            </div>
          </Col>
        </Row>

        <Auth show={showAuth} onHide={() => setShowAuth(false)} />

        <Modal show={!!gameStatus} onHide={continueGame} centered backdrop="static">
          <Modal.Header><Modal.Title>Игра окончена</Modal.Title></Modal.Header>
          <Modal.Body className="text-center">
            {gameStatus === 'PlayerWin' && <h4 className="text-success">Вы выиграли!</h4>}
            {gameStatus === 'BotWin' && <h4 className="text-danger">Вы проиграли.</h4>}
            {gameStatus === 'Draw' && <h4 className="text-secondary">Ничья.</h4>}
          </Modal.Body>
          <Modal.Footer>
            <Button variant="success" onClick={startNewGame}>Начать новую игру</Button>
          </Modal.Footer>
        </Modal>

        <Modal show={showStats} onHide={() => setShowStats(false)} centered size="lg">
          <Modal.Header closeButton><Modal.Title>Статистика</Modal.Title></Modal.Header>
          <Modal.Body>
            <h5>Общая статистика</h5>
            <ul><li>Побед: {stats.global?.wins}</li><li>Поражений: {stats.global?.losses}</li><li>Ничьих: {stats.global?.draws}</li></ul>
            <h5>По сложности</h5>
            <ul>{stats.difficulty?.map(d => <li key={d.difficulty}>Ур. {d.difficulty}: {d.count} игр</li>)}</ul>
            <h5>Макс. серия побед</h5><p>{stats.userMaxStreak}</p>
            <h5>По сложности (пользователь)</h5>
            <ul>{stats.userByDiff?.map(d => <li key={d.difficulty}>Ур. {d.difficulty}: {d.wins}–{d.losses}–{d.draws}</li>)}</ul>
            <h5>Недавние игры</h5>
            <ul><li>Всего: {stats.userRecent?.totalGames}</li><li>Побед: {stats.userRecent?.wins}</li><li>Поражений: {stats.userRecent?.losses}</li><li>Ничьих: {stats.userRecent?.draws}</li><li>Процент: {stats.userRecent?.winPercentage}%</li></ul>
          </Modal.Body>
        </Modal>

        <Modal show={showContinueModal} onHide={() => {}} centered backdrop="static">
          <Modal.Header><Modal.Title>Есть незаконченная игра</Modal.Title></Modal.Header>
          <Modal.Body>Хотите продолжить предыдущую игру?</Modal.Body>
          <Modal.Footer>
            <Button variant="danger" onClick={handleDiscard}>Начать новую</Button>
            <Button variant="primary" onClick={handleContinue}>Продолжить</Button>
          </Modal.Footer>
        </Modal>
      </Card>
    </Container>
  );
};

export default GamePage;