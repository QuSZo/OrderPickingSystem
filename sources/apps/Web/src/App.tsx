import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css'
import { Menu } from './components/Menu/Menu';
import HomePage from './pages/HomePage/HomePage';
import RobotStatusPage from './pages/RobotStatusPage/RobotStatusPage';
import OrderPage from './pages/OrderPage/OrderPage';

function App() {
  return (
    <>
      <BrowserRouter>
        <Menu />
        <main className='main'>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/robot-status" element={<RobotStatusPage />} />
            <Route path="/order-page" element={<OrderPage />} />
          </Routes>
        </main>
      </BrowserRouter>
    </>
  );
}

export default App
