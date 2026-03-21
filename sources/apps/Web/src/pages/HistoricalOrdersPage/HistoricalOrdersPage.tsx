import { useEffect, useState } from 'react';
import { algorithms, type Order, type OrderDto, type OrderedProduct, type TspAlgorithms } from '../../types/Types';
import styles from './HistoricalOrdersPage.module.css'
import { API_URL } from '../../api/const';
import { toast, ToastContainer } from 'react-toastify';

const ORDERS_API_URL = API_URL + "api/orders";

const formatDuration = (ms: number) => {
    const h = Math.floor(ms / 3600000);
    ms %= 3600000;

    const m = Math.floor(ms / 60000);
    ms %= 60000;

    const s = Math.floor(ms / 1000);
    ms %= 1000;

    const parts = [];

    if (h > 0) parts.push(`${h}h`);
    if (m > 0) parts.push(`${m}m`);
    if (s > 0) parts.push(`${s}s`);
    if (ms > 0 || parts.length === 0) parts.push(`${ms}ms`);

    return parts.join(' ');
}

export default function HistoricalOrdersPage() {
    const [historicalOrders, setHistoricalOrders] = useState<Order[]>([]);
    const [expandedIndex, setExpandedIndex] = useState<number | null>(null);
    const [selectedAlgorithm, setSelectedAlgorithm] = useState<TspAlgorithms>("Naive");

    const loadOrders = () => {
        fetch(ORDERS_API_URL)
            .then((response) => response.json())
            .then((data: Order[]) => setHistoricalOrders(data))
            .catch((error) => console.error("Błąd podczas fetch:", error));
    }

    useEffect(() => {
        loadOrders();
    }, []);

    const orderAgain = async (orderedProducts: OrderedProduct[]) => {
        try {
            const order: OrderDto = {orderedProducts: orderedProducts, tspAlgorithm: selectedAlgorithm, timestamp: Date.now()}

            const response = await fetch(`${ORDERS_API_URL}/buy`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(order)
            });

            if (!response.ok) {
                throw new Error("Server error");
            }

            toast.success("Zakup zakończony sukcesem", {
                position: "top-center",
                autoClose: 2000
            });
        }
        catch (error) {
            toast.error("Nie udało się wykonać zakupu", {
                position: "top-center",
                autoClose: 2000
            });

            console.error(error);
        }

        loadOrders();
    };

    const calculatePickingTime = (startPickingTime: number, finishPickingTime: number | null) => {
        if (finishPickingTime !== null) {
            return formatDuration(finishPickingTime - startPickingTime);
        }
        return "Oczekiwanie na wynik";
    }

    return (
        <>
            <ToastContainer />
            <div className={styles.container}>
                <div className={styles.leftContainer}>
                    <table className={styles.table}>
                        <thead>
                            <tr>
                                <th className={styles.arrowTh}></th>
                                <th>Produkty</th>
                                <th>Data zamówienia</th>
                                <th>Wykorzystany algorytm</th>
                                <th>Czas trwania zbierania</th>
                                <th>Przejechana odległość</th>
                                <th className={styles.actionsTh}>Czy zamówić ponownie?</th>
                            </tr>
                        </thead>
                        <tbody>
                        {historicalOrders.map((order, index) => (
                            <>
                                <tr key={index}>
                                    <td onClick={() => setExpandedIndex(expandedIndex === index ? null : index)}>
                                        <span className={styles.arrow}>
                                            {expandedIndex === index ? "▲" : "▼"}
                                        </span></td>
                                    <td className={styles.productsPreviewTd} onClick={() => setExpandedIndex(expandedIndex === index ? null : index)}>
                                        {order.orderedProducts.map(orderedProducts => orderedProducts.name).join(", ")}
                                    </td>
                                    <td>{new Date(order.timestamp).toLocaleString()}</td>
                                    <td>{order.tspAlgorithm}</td>
                                    <td>{calculatePickingTime(order.startPickingTime, order.finishPickingTime)}</td>
                                    <td>Brak danych</td>
                                    <td>
                                        <button className={styles.buttonOrderAgain} onClick={() => orderAgain(order.orderedProducts)}>Zamów ponownie</button>
                                    </td>
                                </tr>

                                {expandedIndex === index && (
                                    <tr>
                                        <td colSpan={7} className={styles.productCellAllInformation}>
                                            <ul>
                                                {order.orderedProducts.map((orderedProduct, index) => (
                                                    <li key={index}>
                                                        {orderedProduct.name} (x:{orderedProduct.position.x} y:{orderedProduct.position.y}) — ilość: {orderedProduct.quantity}
                                                    </li>
                                                ))}
                                            </ul>
                                        </td>
                                    </tr>
                                )}
                            </>
                        ))}
                        </tbody>
                    </table>
                </div>
                <div className={styles.rightContainer}>
                    <h3>Wybór algorytmu</h3>
                    <select value={selectedAlgorithm} onChange={(e) => setSelectedAlgorithm(e.target.value as TspAlgorithms)}>
                        {algorithms.map((alg) => (
                            <option key={alg} value={alg}>
                                {alg}
                            </option>
                        ))}
                    </select>
                </div>
            </div>
        </>
    );
}