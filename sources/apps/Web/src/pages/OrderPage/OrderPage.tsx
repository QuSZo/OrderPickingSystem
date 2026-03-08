import { useEffect, useState } from 'react';
import styles from './OrderPage.module.css'
import { API_URL } from '../../api/const';

const PRODUCTS_API_URL = API_URL + "api/products";
const ORDERS_API_URL = API_URL + "api/orders";

type Position = {
    x: number;
    y: number;
};

type Product = {
    id: string;
    name: string;
    position: Position;
};

export default function OrderPage() {
    const [products, setProducts] = useState<Product[]>([])
    const [selectedProducts, setSelectedProducts] = useState<number[]>([]);

    useEffect(() => {
        fetch(PRODUCTS_API_URL)
            .then((response) => response.json())
            .then((data: Product[]) => setProducts(data))
            .catch((error) => console.error("Błąd podczas fetch:", error));
    }, []);

    const toggleProduct = (index: number) => {
        setSelectedProducts(prev =>
            prev.includes(index) ? prev.filter(i => i !== index) : [...prev, index]
        );
    };

    const buy = async () => {
        const selected = products.filter((_, index) =>
            selectedProducts.includes(index)
        );

        await fetch(ORDERS_API_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(selected)
        });
    };

    const selectedList = selectedProducts.map(index => products[index]);

    return (
        <div className={styles.container}>
            <div className={styles.leftContainer}>
                <table className={styles.table}>
                    <thead>
                        <tr className={styles.tableRow}>
                            <th>Produkt</th>
                            <th>Pozycja</th>
                            <th>Czy pobrać?</th>
                        </tr>
                    </thead>
                    <tbody>
                    {products.map((product, index) => (
                        <tr key={index}>
                            <td>{product.name}</td>
                            <td>x:{product.position.x} y:{product.position.y}</td>
                            <td>
                                <button onClick={() => toggleProduct(index)} className={selectedProducts.includes(index) ? styles.buttonRemove : styles.buttonAdd}>
                                    {selectedProducts.includes(index) ? "Usuń" : "Dodaj"}
                                </button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
            <div className={styles.rightContainer}>
                <div className={styles.orderList}>
                    <h3>Lista zakupów</h3>
                    {selectedList.length === 0 ? (
                        <p>Brak produktów</p>
                    ) : (
                        <ul>
                            {selectedList.map((product, index) => (
                                <li key={index}>
                                    {product.name} (x:{product.position.x} y:{product.position.y})
                                </li>
                            ))}
                        </ul>
                    )}
                </div>
                <div className={styles.buttonContainer}>
                    <button onClick={buy} className={styles.buttonAdd} disabled={selectedList.length === 0}>Zatwiedź zakupy</button>
                </div>
            </div>
        </div>
    );
}