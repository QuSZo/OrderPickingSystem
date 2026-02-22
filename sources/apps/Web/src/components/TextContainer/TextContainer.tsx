import styles from './TextContainer.module.css'

interface TextContainerProps {
    title: string;
    text: string;
}

export function TextContainer({title, text} : TextContainerProps) {
    return (
        <div className={styles.container}>
            <p>{title}</p>
            <div className={styles.box}>
                <p>{text}</p>
            </div>
        </div>

  );
}
