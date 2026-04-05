import { useState } from 'react';
import { API_URL } from '../../api/const';
import styles from './ControlRobotPage.module.css'
import type { RobotCommand, RobotMove } from '../../types/Types';

const ROBOT_COMMAND_API_URL = API_URL + "api/robot/command";
const ROBOT_STOP_API_URL = API_URL + "api/robot/stop";

export default function RobotStatusPage() {
    const [commands, setCommands] = useState<RobotCommand[]>([]);

    const addCommand = (move: RobotMove) => {
        const command: RobotCommand = { move: move, stopDurationMs: undefined}
        setCommands((prev) => [...prev, command]);
    };

    const resetCommands = () => {
        setCommands([]);
    }

    const sendCommands = async () => {
        await fetch(ROBOT_COMMAND_API_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ commands })
        });

        setCommands([]);
        alert("Wysłano");
    };

    const handleStop = async () => {
        await fetch(ROBOT_STOP_API_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
        });

        alert("Zatrzymano");
    }

    return (
        <div className={styles.container}>
            <div className={styles.robotControl}>
                <h2>Manualne sterowanie robotem</h2>
                <div className={styles.addMovesContainer}>
                    <h3>Wybierz komendy</h3>
                    <div className={styles.buttonsContainer}>
                        <button className={styles.normalButton} onClick={() => addCommand("forward")}>Forward</button>
                        <button className={styles.normalButton} onClick={() => addCommand("back")}>Back</button>
                        <button className={styles.normalButton} onClick={() => addCommand("left")}>Left</button>
                        <button className={styles.normalButton} onClick={() => addCommand("right")}>Right</button>
                    </div>
                </div>

                <div className={styles.selectedCommandsContainer}>
                    <h3>Wybrane komendy:</h3>
                    <p>{commands.join(", ") || "Brak komend"}</p>
                </div>

                <div className={styles.buttonsContainer}>
                    <button className={styles.normalButton} onClick={resetCommands}>Reset</button>
                    <button className={styles.addButton} onClick={sendCommands} disabled={commands.length === 0}>Wyślij</button>
                    <button className={styles.stopButton} onClick={handleStop}>Stop</button>
                </div>
            </div>
        </div>
    );
}