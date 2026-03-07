import { Link } from "react-router-dom";
import styles from './Menu.module.css'

export function Menu() {
  return (
    <div className={styles.container}>
      <nav className={styles.menu}>
        <Link to="/">Start</Link>
        <Link to="/robot-status">Status robota</Link>
        <Link to="/order-page">Zamówienia</Link>
      </nav>
    </div>
  );
}
