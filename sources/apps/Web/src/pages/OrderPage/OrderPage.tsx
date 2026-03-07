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

    const buy = () => {

    }

    return (
        <div className={styles.container}>
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
                        <td>Tak / Nie</td>
                    </tr>
                ))}
                </tbody>
            </table>
            <div className={styles.buttonContainer}>
                <button onClick={buy}>Zatwiedź zakupy</button>
            </div>
        </div>
    );
}