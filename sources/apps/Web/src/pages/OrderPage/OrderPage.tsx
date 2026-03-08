import { useState } from 'react';
import styles from './OrderPage.module.css'

type Products = {
    name: string,
    position: string
}

const products: Products[] = [
    { name: "woda", position: "x:0 y: 0" },
    { name: "chleb", position: "x:0 y: 1" },
    { name: "masło", position: "x:0 y: 2" },
    { name: "cukier", position: "x:0 y: 3" },
    { name: "sól", position: "x:0 y: 4" },
    { name: "ryż", position: "x:0 y: 0" },
    { name: "makaron", position: "x:1 y: 1" },
    { name: "ziemniaki", position: "x:1 y: 2" },
    { name: "kapusta", position: "x:1 y: 3" },
    { name: "szynka", position: "x:1 y: 4" },
    { name: "ser", position: "x:2 y: 0" },
    { name: "twaróg", position: "x:2 y: 1" },
] 

export default function OrderPage() {
    const [selectedProducts, setSelectedProducts] = useState<number[]>([]);

    const toggleProduct = (index: number) => {
        setSelectedProducts(prev =>
            prev.includes(index) ? prev.filter(i => i !== index) : [...prev, index]
        );
    };

    const buy = async () => {
        const selected = products.filter((_, index) =>
            selectedProducts.includes(index)
        );

        await fetch("http://localhost:8080/api/orders", {
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
                            <td>{product.position}</td>
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
                                    {product.name} ({product.position})
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