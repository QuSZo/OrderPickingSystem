import { useEffect, useState } from 'react';
import { algorithms, type OrderDto, type CreateOrderCommand, type OrderedProduct, type TspAlgorithms } from '../../types/Types';
import styles from './HistoricalOrdersPage.module.css'
import { API_URL } from '../../api/const';
import { toast, ToastContainer } from 'react-toastify';
import { formatDuration } from '../../utils/TimeUtils';

const ORDERS_API_URL = API_URL + "api/orders";

export default function HistoricalOrdersPage() {
    const [historicalOrders, setHistoricalOrders] = useState<OrderDto[]>([]);
    const [expandedIndex, setExpandedIndex] = useState<number | null>(null);

    const loadOrders = () => {
        fetch(ORDERS_API_URL)
            .then((response) => response.json())
            .then((data: OrderDto[]) => setHistoricalOrders(data))
            .catch((error) => console.error("Błąd podczas fetch:", error));
    }

    useEffect(() => {
        loadOrders();
    }, []);

    const orderAgain = async (orderedProducts: OrderedProduct[], algorithm: TspAlgorithms) => {
        try {
            // TODO: remove line below, only for testing purposes
            const orderWithSingleQuantity = orderedProducts.map(orderedProduct =>     
            ({
                ...orderedProduct, quantity: 1
            }))
            const order: CreateOrderCommand = {orderedProducts: orderWithSingleQuantity, tspAlgorithm: algorithm, timestamp: Date.now()}

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

    const calculatePickingTime = (startPickingTime: number | null, finishPickingTime: number | null) => {
        if (startPickingTime !== null && finishPickingTime !== null) {
            return formatDuration(finishPickingTime - startPickingTime);
        }
        return "Oczekiwanie na wynik";
    }

    const calculateAverageSpeed = (startPickingTime: number | null, finishPickingTime: number | null, distance: number) => {
        if (startPickingTime !== null && finishPickingTime !== null) {
            const timeInSeconds = (finishPickingTime - startPickingTime) / 1000;
            const speed = distance / timeInSeconds;
            return `${speed.toFixed(2)} m/s`;
        }
        return "Oczekiwanie na wynik";
    }

    return (
        <>
            <ToastContainer />
            <div className={styles.container}>
                <table className={styles.table}>
                    <thead>
                        <tr>
                            <th className={styles.arrowTh}></th>
                            <th>Produkty</th>
                            <th>Data zamówienia</th>
                            {/* TODO: remove 6 lines below, only for testing purposes */}
                            <th>Data rozpoczenia</th>
                            <th>Data zakończenia</th>
                            <th>PropAbsMean</th>
                            <th>DerAbsMean</th>
                            <th>IntegAbsMean</th>
                            <th>PowerDiffAbsMean</th>
                            <th>Wykorzystany algorytm</th>
                            <th>Czas trwania zbierania</th>
                            <th>Przejechana odległość</th>
                            <th>Średnia prędkość</th>
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
                                {/* TODO: remove 6 lines below, only for testing purposes */}
                                <td>{order.startPickingTime}</td>
                                <td>{order.finishPickingTime}</td>
                                <td>{order.proportionalAbsoluteMean}</td>
                                <td>{order.derivativeAbsoluteMean}</td>
                                <td>{order.integralAbsoluteMean}</td>
                                <td>{order.powerDifferenceAbsoluteMean}</td>
                                <td>{order.tspAlgorithm}</td>
                                <td>{calculatePickingTime(order.startPickingTime, order.finishPickingTime)}</td>
                                <td>{order.distance} cm</td>
                                <td>{calculateAverageSpeed(order.startPickingTime, order.finishPickingTime, order.distance)}</td>
                                <td>
                                    <select className={styles.select} defaultValue="" onChange={(e) => {orderAgain(order.orderedProducts, e.target.value as TspAlgorithms); e.target.value=""}}>
                                        <option value="" disabled>Zamów ponownie</option>
                                        {algorithms.map((alg) => (
                                            <option key={alg} value={alg}>{alg}</option>
                                        ))}
                                    </select>
                                </td>
                            </tr>

                            {expandedIndex === index && (
                                <tr>
                                    <td colSpan={8} className={styles.productCellAllInformation}>
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
        </>
    );
}