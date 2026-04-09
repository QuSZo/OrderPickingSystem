import styles from './TextContainer.module.css'

interface TextContainerProps {
    text?: string;
}

export function TextContainer({text} : TextContainerProps) {
    return (
        <p className={styles.boxText}>{text ?? "Brak danych"}</p>
  );
}
