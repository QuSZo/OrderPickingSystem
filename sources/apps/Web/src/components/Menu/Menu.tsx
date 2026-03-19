import { Link } from "react-router-dom";
import styles from './Menu.module.css'
import aghLogo from "../../assets/agh.svg";

export function Menu() {
  return (
    <div className={styles.container}>
      <img src={aghLogo} alt="AGH logo" className={styles.aghLogo}></img>
      <nav className={styles.menu}>
        <Link to="/">Start</Link>
        <Link to="/order-page">Zamówienia</Link>
        <Link to="/robot-status">Status robota</Link>
        <Link to="/robot-control">Manualne sterowanie</Link>
      </nav>
    </div>
  );
}
