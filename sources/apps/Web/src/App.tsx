import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css'
import { Menu } from './components/Menu/Menu';
import Page1 from './pages/Page1/Page1';
import Page2 from './pages/Page2/Page2';

function App() {
  return (
    <>
      <BrowserRouter>
        <Menu />
        <main className='main'>
          <Routes>
            <Route path="/" element={<Page1 />} />
            <Route path="/Page2" element={<Page2 />} />
          </Routes>
        </main>
      </BrowserRouter>
    </>
  );
}

export default App
