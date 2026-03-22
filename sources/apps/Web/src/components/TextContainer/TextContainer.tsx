import styles from './TextContainer.module.css'

interface TextContainerProps {
    title: string;
    text?: string;
}

export function TextContainer({title, text} : TextContainerProps) {
    return (
        <div className={styles.container}>
            <p className={styles.title}>{title}</p>
            <div className={styles.box}>
                <p className={styles.boxText}>{text ?? "Brak danych"}</p>
            </div>
        </div>

  );
}
