import { BrowserRouter, Link, Route, Routes } from 'react-router-dom';
import './App.css'
import Page1 from './pages/Page1';
import Page2 from './pages/Page2';

function App() {
  return (
    <BrowserRouter>
        <nav>
          <Link to="/">Page1</Link>
          <Link to="/Page2">Page2</Link>
        </nav>

        <Routes>
          <Route path="/" element={<Page1 />} />
          <Route path="/Page2" element={<Page2 />} />
        </Routes>
    </BrowserRouter>
  );
}

export default App
