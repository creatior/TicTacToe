import { useState, useContext } from 'react';
import { Button, Modal, Form } from 'react-bootstrap';
import { Context } from '../index';
import { login, registration } from '../services/userAPI';

function Auth({ show, onHide, onLogin }) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [isLogin, setIsLogin] = useState(true); // теперь состояние
  const { user } = useContext(Context);

  const click = async () => {
    try {
      let data;
      if (isLogin) {
        data = await login(username, password);
      } else {
        data = await registration(username, password);
      }

      user.setUser(username); // если API вернёт `user`
      user.setIsAuth(true);
      onHide();
    } catch (error) {
      alert('Ошибка: ' + error.message);
    }
  };

  return (
    <Modal show={show} onHide={onHide} backdrop="static" keyboard={false} centered>
      <Modal.Header>
        <Modal.Title>{isLogin ? 'Вход' : 'Регистрация'}</Modal.Title>
      </Modal.Header>

      <Modal.Body>
        <Form>
          <Form.Group className="mb-3">
            <Form.Control
              value={username}
              onChange={e => setUsername(e.target.value)}
              placeholder="Имя пользователя"
              required
            />
          </Form.Group>

          <Form.Group>
            <Form.Control
              value={password}
              onChange={e => setPassword(e.target.value)}
              placeholder="Пароль"
              type="password"
              required
            />
          </Form.Group>
        </Form>
      </Modal.Body>

      <Modal.Footer className="d-flex justify-content-between align-items-center">
        <div
          role="button"
          onClick={() => setIsLogin(!isLogin)}
          className="text-primary"
        >
          {isLogin ? 'Нет аккаунта? Зарегистрироваться!' : 'Есть аккаунт? Войти!'}
        </div>

        <Button onClick={click}>
          {isLogin ? 'Войти' : 'Зарегистрироваться'}
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

export default Auth;
