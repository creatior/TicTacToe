import {createContext} from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import GamePage from './pages/GamePage';
import User from './store/User'

export const Context = createContext(null)

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <Context.Provider value={{
    user: new User(),
  }}>
  <GamePage />
  </Context.Provider>
);
