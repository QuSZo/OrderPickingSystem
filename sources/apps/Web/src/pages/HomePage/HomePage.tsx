import { useState, useEffect } from "react";
import styles from './HomePage.module.css'
import { API_URL } from "../../api/const";
import { Link } from "react-router-dom";

const HOME_API_URL = API_URL + "api/home";

export default function HomePage() {
  const [data, setData] = useState("");

  useEffect(() => {
    fetch(HOME_API_URL)
      .then((response) => response.text())
      .then((text) => setData(text))
      .catch((error) => console.error("Błąd podczas fetch:", error));
  }, []);

  return (
    <div className={styles.container}>
      <h2>Autonomiczny system kompletacji zamówień</h2>
      <div className={styles.routeContainer}>
        <p>Jeśli chcesz rozpocząć zamawianie kliknij&nbsp;</p>
        <Link to="/order-page">Zamówienia</Link>
      </div>
      <div className={styles.routeContainer}>
        <p>Jeśli chcesz sprawdzić status rozpoczętego zamówienia kliknij&nbsp;</p>
        <Link to="/robot-status">Status robota</Link>
      </div>
      <div className={styles.apiStatusContainer}>
        <div className={styles.apiStatusConfirmation}>
          <p>Czy nawiązano połączenie z API?&nbsp;</p>
          {data == "" ? (
            <p className={styles.denialText}>NIE</p>
          ) : (
              <p className={styles.confirmationText}>TAK</p>
          )}
        </div>
        {data == "" ? "" : <p>Pobrane dane: {data}</p>}
      </div>
    </div>
  );
}