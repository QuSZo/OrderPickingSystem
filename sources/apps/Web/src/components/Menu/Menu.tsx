import { Link } from "react-router-dom";
import styles from './Menu.module.css'

export function Menu() {
  return (
    <div className={styles.container}>
      <nav className={styles.menu}>
        <Link to="/">Page1</Link>
        <Link to="/Page2">Page2</Link>
      </nav>
    </div>
  );
}
