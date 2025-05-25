import React, { useContext, useState, useEffect } from 'react';
import { Container, Row, Col, Button, Dropdown, Card, Modal } from 'react-bootstrap';
import Auth from '../components/Auth';
import { Context } from '..';
import { update, create, move, fetchStats } from '../services/gameAPI';
import bot from '../assets/primate.jpg'

const EMPTY_BOARD = Array(16).fill(0);

const GamePage = () => {
  const [showAuth, setShowAuth] = useState(false);
  const [difficulty, setDifficulty] = useState('1');
  const [board, setBoard] = useState(EMPTY_BOARD);
  const [isLocked, setIsLocked] = useState(false);
  const [gameStatus, setGameStatus] = useState('BotWin');
  const [showStats, setShowStats] = useState(false);
  const [stats, setStats] = useState({
    games: 0,
    wins: 0,
    losses: 0,
    draws: 0,
    winRate: 0,
  });
  const { user } = useContext(Context);

  // useEffect(() => {
  //   if (!user.isAuth) setShowAuth(true);
  // }, [user.isAuth]);

  useEffect(() => {
    if (showStats) {
      fetchStats();
    }
  }, [showStats]);

  const startNewGame = () => {
    setBoard(EMPTY_BOARD);
    setGameStatus(null);
    setIsLocked(false);
  };

  const continueGame = () => {
    setGameStatus(null)
  };

  const handleCellClick = async (index) => {
    if (board[index] !== 0 || isLocked || gameStatus) return;

    const newBoard = [...board];
    newBoard[index] = 1;
    setBoard(newBoard);
    setIsLocked(true);

    try {
      const {board: updatedBoard, status} = await move(board, difficulty);
      
      setBoard(updatedBoard);
      setGameStatus(status || null);

    } catch (error) {
      console.error('Ошибка при запросе хода бота:', error);
    } finally {
      setIsLocked(false);
    }
  };

  return (
    <Container className="mt-4 d-flex justify-content-center align-items-center" style={{ minHeight: '100vh' }}>
      <Card className="p-5 w-100" style={{ maxWidth: '1400px' }}>
        <Row className="mb-3">
          <Col><h1>Крестики-нолики 4x4</h1></Col>
        </Row>

        <Row>
          <Col xs={14} md={3} className="d-flex flex-column justify-content-center align-items-center mb-3 mb-md-0">
            <img 
              src={bot} 
              alt="Game Icon" 
              style={{ width: '250px', height: '250px', objectFit: 'contain' }} 
            />
            <h3>Ваш оппонент:</h3>
          </Col>
          <Col xs={14} md={6} className="mb-4 mb-md-0">
            <div
              style={{
                display: 'grid',
                gridTemplateColumns: 'repeat(4, minmax(100px, 1fr))',
                gap: '2px',
              }}
            >
              {Array.isArray(board) && board.map((cell, i) => (
                <Button
                  key={i}
                  variant="outline-dark"
                  style={{ height: '100px', width: '100%', fontSize: '1.5rem' }}
                  onClick={() => handleCellClick(i)}
                  disabled={isLocked || gameStatus}
                >
                  {cell === 1 ? 'X' : cell === 2 ? 'O' : ''}
                </Button>
              ))}
            </div>
          </Col>

          <Col xs={14} md={3} className="d-flex flex-column justify-content-center">
            <div className="mb-4">
              <h4>Уровень сложности</h4>
              <Dropdown onSelect={(e) => setDifficulty(e)}>
                <Dropdown.Toggle variant="secondary" style={{ width: '100%' }}>{difficulty}</Dropdown.Toggle>
                <Dropdown.Menu>
                  <Dropdown.Item eventKey="Primate">Primate</Dropdown.Item>
                  <Dropdown.Item eventKey="Easy">Easy</Dropdown.Item>
                  <Dropdown.Item eventKey="Medium">Medium</Dropdown.Item>
                  <Dropdown.Item eventKey="Hard">Hard</Dropdown.Item>
                </Dropdown.Menu>
              </Dropdown>
            </div>

            <div className="mb-4 d-grid gap-2">
              <Button onClick={startNewGame}>
                Новая игра
              </Button>
              {user.isAuth ? (
                <Button onClick={() => setShowAuth(true)}>
                  Войти
                </Button>
              ) : (
                <Button onClick={() => setShowAuth(true)}>
                  Сменить аккаунт
                </Button>
              )}
              <Button onClick={() => setShowStats(true)}>
                Статистика
              </Button>
            </div>

          </Col>
        </Row>

        <Auth show={showAuth} onHide={() => setShowAuth(false)} />

        <Modal show={!!gameStatus} onHide={() => setGameStatus(null)} centered backdrop="static">
          <Modal.Header>
            <Modal.Title>Игра окончена</Modal.Title>
          </Modal.Header>
          <Modal.Body className="text-center">
            {gameStatus === 'PlayerWin' && <h4 className="text-success">Вы выиграли!</h4>}
            {gameStatus === 'BotWin' && <h4 className="text-danger">Вы проиграли.</h4>}
            {gameStatus === 'Draw' && <h4 className="text-secondary">Ничья.</h4>}
            {gameStatus === 'Continue' && <h4 className="text-secondary">Предыдущая игра не окончена. Продолжить?</h4>}
          </Modal.Body>
          <Modal.Footer>
            {gameStatus === 'Continue' && 
              <>
              <Button variant='outline-danger' onClick={startNewGame}>Начать новую игру</Button>
              <Button variant="success" onClick={continueGame}>Продолжить</Button>
              </>
            }
            {gameStatus != 'Continue' && <Button variant="success" onClick={startNewGame}>Начать новую игру</Button>}
          </Modal.Footer>
        </Modal>

        <Modal show={showStats} onHide={() => setShowStats(false)} centered>
          <Modal.Header closeButton>
            <Modal.Title>Статистика игрока</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <ul>
              <li>Игр сыграно: {stats.games}</li>
              <li>Побед: {stats.wins}</li>
              <li>Поражений: {stats.losses}</li>
              <li>Ничьих: {stats.draws}</li>
              <li>Процент побед: {stats.winRate}%</li>
            </ul>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={() => setShowStats(false)}>Закрыть</Button>
          </Modal.Footer>
        </Modal>

      </Card>
    </Container>
  );
};

export default GamePage;