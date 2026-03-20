import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css'
import { Menu } from './components/Menu/Menu';
import HomePage from './pages/HomePage/HomePage';
import RobotStatusPage from './pages/RobotStatusPage/RobotStatusPage';
import OrderPage from './pages/OrderPage/OrderPage';
import ControlRobotPage from './pages/ControlRobotPage/ControlRobotPage';
import HistoricalOrdersPage from './pages/HistoricalOrdersPage/HistoricalOrdersPage';

function App() {
  return (
    <>
      <BrowserRouter>
        <Menu />
        <main className='main'>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/order-page" element={<OrderPage />} />
            <Route path="/order-history" element={<HistoricalOrdersPage />} />
            <Route path="/robot-status" element={<RobotStatusPage />} />
            <Route path="/robot-control" element={<ControlRobotPage />} />
          </Routes>
        </main>
      </BrowserRouter>
    </>
  );
}

export default App
