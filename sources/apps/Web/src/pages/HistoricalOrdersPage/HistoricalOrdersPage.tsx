import { useEffect, useState } from 'react';
import type { OrderedProduct } from '../../types/Types';
import styles from './HistoricalOrdersPage.module.css'
import { API_URL } from '../../api/const';
import { toast, ToastContainer } from 'react-toastify';

const ORDERS_API_URL = API_URL + "api/orders";

export default function HistoricalOrdersPage() {
    const [historicalOrders, setHistoricalOrders] = useState<OrderedProduct[][]>([]);
    const [expandedIndex, setExpandedIndex] = useState<number | null>(null);

    const loadOrders = () => {
        fetch(ORDERS_API_URL)
            .then((response) => response.json())
            .then((data: OrderedProduct[][]) => setHistoricalOrders(data))
            .catch((error) => console.error("Błąd podczas fetch:", error));
    }

    useEffect(() => {
        loadOrders();
    }, []);

    const orderAgain = async (orderedProducts: OrderedProduct[]) => {
        try {
            const response = await fetch(`${ORDERS_API_URL}/buy`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(orderedProducts)
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

    return (
        <>
            <ToastContainer />
            <div className={styles.container}>
                <div className={styles.leftContainer}>
                    <table className={styles.table}>
                        <thead>
                            <tr className={styles.tableRow}>
                                <th className={styles.arrowTh}></th>
                                <th>Produkty</th>
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
                                        {order.map(p => p.name).join(", ")}
                                    </td>
                                    <td>
                                        <button className={styles.buttonOrderAgain} onClick={() => orderAgain(order)}>Zamów ponownie</button>
                                    </td>
                                </tr>

                                {expandedIndex === index && (
                                    <tr>
                                        <td colSpan={3} className={styles.productCellAllInformation}>
                                            <ul>
                                                {order.map((orderedProduct, index) => (
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
                </div>
            </div>
        </>
    );
}