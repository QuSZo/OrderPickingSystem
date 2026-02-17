import { useState, useEffect } from "react";

const API_URL = "http://localhost:8080/api/home";

export default function Page1() {
  const [data, setData] = useState("");

  useEffect(() => {
    fetch(API_URL)
      .then((response) => response.text())
      .then((text) => setData(text))
      .catch((error) => console.error("Błąd podczas fetch:", error));
  }, []);

  return (
    <div>
      <p>Page1</p>
      <p>Pobrane dane: {data}</p>
    </div>
  );
}