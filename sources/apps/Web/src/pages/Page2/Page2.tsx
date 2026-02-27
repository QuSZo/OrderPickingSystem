import styles from './Page2.module.css'
import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { API_URL } from '../../api/const';
import { TextContainer } from '../../components/TextContainer/TextContainer';

const ROBOT_API_URL = API_URL + "api/robot-hub";
const ROBOT_COMMAND_API_URL = API_URL + "api/robot/command";
const ROBOT_STOP_API_URL = API_URL + "api/robot/stop";

type RobotCommand = "forward" | "back" | "left" | "right";

interface RobotEvent {
    event: "movement" | "finished" | "stopped";
    timestamp: number;
    command?: string;
}

export default function Page2() {
    const [robotState, setRobotState] = useState<RobotEvent | null>(null);
    const [commands, setCommands] = useState<RobotCommand[]>([]);

    const addCommand = (command: RobotCommand) => {
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

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(ROBOT_API_URL)
            .withAutomaticReconnect()
            .build();

        connection.start()
            .then(() => console.log("Connected"))
            .catch(() => console.error("Failed to connect"))

        connection.on("ReceiveRobotState", (message) => {
            try {
                const parsed: RobotEvent = JSON.parse(message);
                setRobotState(parsed);
                console.log(message);
            } catch (err) {
                console.error("Błąd parsowania", err);
            }
        });

        return () => {
            connection.stop();
        };
    }, []);

    return (
        <div className={styles.container}>
            <div className={styles.robotStatus}>
                <p>Status robota</p>
                <TextContainer title="Akcja" text={robotState?.event}></TextContainer>
                <TextContainer title="Wykonany ruch" text={robotState?.command}></TextContainer>
                <TextContainer title="Ostatnia synchronizacja" text={robotState ? new Date(robotState.timestamp * 1000).toLocaleString() : undefined}></TextContainer>
            </div>
            
            <div className={styles.robotControl}>
                <p>Sterowanie robotem</p>
                <div>
                    <button onClick={() => addCommand("forward")}>Forward</button>
                    <button onClick={() => addCommand("back")}>Back</button>
                    <button onClick={() => addCommand("left")}>Left</button>
                    <button onClick={() => addCommand("right")}>Right</button>
                </div>

                <div>
                    <h4>Wybrane komendy:</h4>
                    <p>{commands.join(", ") || "Brak komend"}</p>
                </div>

                <div>
                    <button onClick={resetCommands}>Reset</button>
                    <button onClick={sendCommands} disabled={commands.length === 0}>Wyślij</button>
                    <button onClick={handleStop}>Stop</button>
                </div>
            </div>


        </div>
    );
}