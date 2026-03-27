import { useEffect, useState } from 'react';
import styles from './OrderPage.module.css'
import { API_URL } from '../../api/const';
import { ToastContainer, toast } from "react-toastify";
import { algorithms, type OrderDto, type OrderedProduct, type Product, type TspAlgorithms } from '../../types/Types';
import { WarehouseMapCols, WarehouseMapRows, WarehouseMapStops } from '../../const/const';
import { getRandomElements } from '../../utils/ArrayUtils';

const PRODUCTS_API_URL = API_URL + "api/products";
const ORDERS_API_URL = API_URL + "api/orders";

type RandomMode = "random" | "cluster"

export default function OrderPage() {
    const [products, setProducts] = useState<Product[]>([])
    const [orderedProducts, setOrderedProducts] = useState<OrderedProduct[]>([]);
    const [selectedAlgorithm, setSelectedAlgorithm] = useState<TspAlgorithms>("Zachłanny");
    const [productCountToRandomSelect, setProductCountToRandomSelect] = useState<number>(1);
    const [randomMode, setRandomMode] = useState<RandomMode>("random");
    const [clusterCountToRandomSelect, setClusterCountToRandomSelect] = useState<number>(1);

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

    const handleRandom = () => {
        if (randomMode === "random") {
            randomlySelectProducts();
        } else {
            randomlySelectClusteredProducts();
        }
    };

    const randomlySelectProducts = () => {
        if (products.length === 0) return;

        const productCountToSelect = Math.min(productCountToRandomSelect, products.length);

        const selected = getRandomElements(products, productCountToSelect)

        const randomOrdered: OrderedProduct[] = selected.map(p => ({
            id: p.id,
            name: p.name,
            position: p.position,
            quantity: Math.floor(Math.random() * 5) + 1
        }));

        setOrderedProducts(randomOrdered);
    };

    const randomlySelectClusteredProducts = () => {
        if (products.length === 0) return;

        const aisles: Product[][] = createAisles();
        if (aisles.length === 0) return;

        const productCountToSelect = Math.min(productCountToRandomSelect, products.length);
        const clusterCount = Math.min(clusterCountToRandomSelect, aisles.length);

        const selectedAisles = getRandomElements(aisles, clusterCount)

        const neededProductsPerAisle = Math.floor(productCountToSelect / clusterCount);
        let remainderProducts = productCountToSelect % clusterCount;

        const result: Product[] = [];

        selectedAisles.forEach(selectedAisle => {
            let take = neededProductsPerAisle;

            if (remainderProducts > 0) {
                take += 1;
                remainderProducts--;
            }

            const selectedProducts = getRandomElements(selectedAisle, take)

            result.push(...selectedProducts);
        });

        const randomOrdered: OrderedProduct[] = result.map(p => ({
            id: p.id,
            name: p.name,
            position: p.position,
            quantity: Math.floor(Math.random() * 5) + 1
        }));

        setOrderedProducts(randomOrdered);
    };

    const createAisles = () => {
        const aisles: Product[][] = [];

        for (let colNumber = 0; colNumber < WarehouseMapCols; colNumber++) {
            for (let rowNumber = 0; rowNumber < WarehouseMapRows - 1; rowNumber++) {
                const rowStartAisle = rowNumber * (WarehouseMapStops + 1) + 1;
                const productsInAisle : Product[] = []
                for (let i = 0; i < WarehouseMapStops; i++) {
                    const product = products.find(product => product.position.x === colNumber && product.position.y === rowStartAisle + i)
                    if (product) {
                        productsInAisle.push(product);
                    }
                    else {
                        toast.error("Brakuje produktu w alejce. Losowanie nie jest możliwe.", {
                            position: "top-center",
                            autoClose: 2000
                        });
                        return [];
                    }
                }
                aisles.push(productsInAisle);
            }
        }

        return aisles;
    }

    const buy = async () => {
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
                    <div className={styles.randomProductsContainer}>
                        <h3>Losowanie produktów</h3>
                        <div>
                            <label>Tryb losowania: </label>
                            <select value={randomMode} onChange={(e) => {setRandomMode(e.target.value as RandomMode); setProductCountToRandomSelect(1); setClusterCountToRandomSelect(1);}}>
                                <option value="random">Losowy</option>
                                <option value="cluster">Skupiska</option>
                            </select>
                        </div>
                        <div>
                            <label>Liczba produktów do wylosowania: </label>
                            <input type="number" min={1} max={products.length} value={productCountToRandomSelect} onChange={(e) => setProductCountToRandomSelect(Number(e.target.value))}/>
                            {productCountToRandomSelect > products.length && (
                                <p className={styles.error}>
                                    Liczba produktów do wylosowania nie może być większa od wszystkich dostępnych produktów
                                </p>
                            )}
                            {productCountToRandomSelect < 1 && (
                                <p className={styles.error}>
                                    Liczba produktów do wylosowania nie może być mniejsza niż 1
                                </p>
                            )}
                        </div>
                        {randomMode == "cluster" && (
                            <div>
                                <label>Liczba klastrów</label>
                                <input type="number" min={1} max={productCountToRandomSelect} value={clusterCountToRandomSelect} onChange={(e) => setClusterCountToRandomSelect(Number(e.target.value))}/>
                                {clusterCountToRandomSelect > productCountToRandomSelect && (
                                    <p className={styles.error}>
                                        Liczba klastrów nie może być większa niż liczba produktów do wylosowania
                                    </p>
                                )}
                                {clusterCountToRandomSelect < Math.ceil(productCountToRandomSelect / WarehouseMapStops) && (
                                    <p className={styles.error}>
                                        Minimalna liczba klastrów dla {productCountToRandomSelect} produktów to {Math.ceil(productCountToRandomSelect / WarehouseMapStops)}
                                    </p>
                                )}
                                {clusterCountToRandomSelect < 1 && (
                                    <p className={styles.error}>
                                        Liczba klastrów nie może być mniejsza niż 1
                                    </p>
                                )}
                            </div>
                        )}
                        <div>
                            <button onClick={handleRandom} className={styles.buttonAdd} disabled={orderedProducts.length !== 0 || productCountToRandomSelect > products.length || productCountToRandomSelect < 1 || (randomMode === "cluster" && (clusterCountToRandomSelect > productCountToRandomSelect || clusterCountToRandomSelect < Math.ceil(productCountToRandomSelect / WarehouseMapStops) || clusterCountToRandomSelect < 1))}>Wylosuj produkty</button>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}