import { useEffect, useState } from 'react';
import styles from './OrderPage.module.css'
import { API_URL } from '../../api/const';
import { ToastContainer, toast } from "react-toastify";
import { algorithms, type Order, type OrderedProduct, type Product, type TspAlgorithms } from '../../types/Types';

const PRODUCTS_API_URL = API_URL + "api/products";
const ORDERS_API_URL = API_URL + "api/orders";

export default function OrderPage() {
    const [products, setProducts] = useState<Product[]>([])
    const [orderedProducts, setOrderedProducts] = useState<OrderedProduct[]>([]);
    const [selectedAlgorithm, setSelectedAlgorithm] = useState<TspAlgorithms>("Naive");
    const [randomCount, setRandomCount] = useState<number>(1);

    useEffect(() => {
        fetch(PRODUCTS_API_URL)
            .then((response) => response.json())
            .then((data: Product[]) => setProducts(data))
            .catch((error) => console.error("Błąd podczas fetch:", error));
    }, []);

    const selectProduct = (product: Product) => {
        setOrderedProducts(prev => {
            const existing = prev.find(p => p.id === product.id);

            if (!existing) {
                const orderedProduct : OrderedProduct = { id: product.id, name: product.name, position: product.position, quantity: 1 }
                return [...prev, orderedProduct];
            }

            return prev.map(orderedProduct =>
                orderedProduct.id === product.id ? { ...orderedProduct, quantity: orderedProduct.quantity + 1 } : orderedProduct
            );
        });
    };

    const removeOne = (productToRemove: OrderedProduct) => {
        setOrderedProducts(prev => 
            prev.map(product => product.id === productToRemove.id ? { ...product, quantity: product.quantity - 1 } : product)
            .filter(product => product.quantity > 0)
        );
    };

    const removeProduct = (productToRemove: OrderedProduct) => {
        setOrderedProducts(prev => prev.filter(product => product.id !== productToRemove.id));
    };

    const clearOrder = () => {
        setOrderedProducts([]);
    };

    const randomlySelectProducts = () => {
        if (products.length === 0) return;

        const count = Math.min(randomCount, products.length);

        const shuffled = [...products];
        for (let i = shuffled.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
        }

        const selected = shuffled.slice(0, count);

        const randomOrdered: OrderedProduct[] = selected.map(p => ({
            id: p.id,
            name: p.name,
            position: p.position,
            quantity: Math.floor(Math.random() * 10)
        }));

        setOrderedProducts(randomOrdered);
    };

    const buy = async () => {
        try {
            const order: Order = {orderedProducts: orderedProducts, tspAlgorithm: selectedAlgorithm, timestamp: Date.now()}
 
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

            setOrderedProducts([]);
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
    };

    return (
        <>
            <ToastContainer />
            <div className={styles.container}>
                <div className={styles.leftContainer}>
                    <table className={styles.table}>
                        <thead>
                            <tr>
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
                                    <button onClick={() => selectProduct(product)} className={styles.buttonAddProduct}>Dodaj</button>
                                </td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                </div>
                <div className={styles.rightContainer}>
                    <div className={styles.orderList}>
                        <h3>Lista zakupów</h3>
                        {orderedProducts.length === 0 ? (
                            <p>Brak produktów</p>
                        ) : (
                            <table className={styles.table}>
                                <thead>
                                    <tr>
                                        <th>Zakupiony produkt</th>
                                        <th>Ilość</th>
                                        <th className={styles.actionsTh}>Akcje</th>
                                    </tr>
                                </thead>
                                <tbody>
                                {orderedProducts.map((orderedProduct, index) => (
                                    <tr key={index}>
                                        <td>{orderedProduct.name} (x:{orderedProduct.position.x} y:{orderedProduct.position.y})</td>
                                        <td>{orderedProduct.quantity}</td>
                                        <td>
                                            <div className={styles.orderedActionContainer}>
                                                <button onClick={() => removeOne(orderedProduct)} className={styles.buttonProductUpdate}>Usuń jedno</button>
                                                <button onClick={() => removeProduct(orderedProduct)} className={styles.buttonProductRemove}>Usuń produkt</button>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                                </tbody>
                            </table>
                        )}
                    </div>
                    <div className={styles.orderSummaryContainer}>
                        <h2>Podsumowanie</h2>
                        <div className={styles.summary}>
                            <h4>Łącznie produktów: {orderedProducts.length}</h4>
                            <h4>Łączna ilość: {orderedProducts.reduce((sum, product) => sum + product.quantity, 0)}</h4>
                            <div className={styles.algorithmContainer}>
                                <h4>Algorytm</h4>
                                <select value={selectedAlgorithm} onChange={(e) => setSelectedAlgorithm(e.target.value as TspAlgorithms)}>
                                    {algorithms.map((alg) => (
                                        <option key={alg} value={alg}>
                                            {alg}
                                        </option>
                                    ))}
                                </select>
                            </div>
                        </div>
                        <div className={styles.buttonContainer}>
                            <button onClick={() => clearOrder()} className={styles.buttonRemove}>Wyczyść zamówienie</button>
                            <button onClick={buy} className={styles.buttonAdd} disabled={orderedProducts.length === 0}>Zatwiedź zakupy</button>
                        </div>
                    </div>
                    <div>
                        <h3>Losowanie produktów</h3>
                        <div>
                            <label>Liczba produktów do wylosowania: </label>
                            <input type="number" min={1} max={products.length} value={randomCount} onChange={(e) => setRandomCount(Number(e.target.value))}/>
                        </div>
                        <button onClick={randomlySelectProducts} className={styles.buttonAdd} disabled={orderedProducts.length !== 0}>Wylosuj produkty</button>
                    </div>
                </div>
            </div>
        </>
    );
}