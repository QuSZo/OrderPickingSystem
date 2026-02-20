import { useState, useEffect } from "react";
import styles from './Page1.module.css'
import { API_URL } from "../../api/const";

const HOME_API_URL = API_URL + "api/home";

export default function Page1() {
  const [data, setData] = useState("");

  useEffect(() => {
    fetch(HOME_API_URL)
      .then((response) => response.text())
      .then((text) => setData(text))
      .catch((error) => console.error("Błąd podczas fetch:", error));
  }, []);

  return (
    <div className={styles.container}>
      <p>Page1</p>
      <p>Pobrane dane: {data}</p>
    </div>
  );
}