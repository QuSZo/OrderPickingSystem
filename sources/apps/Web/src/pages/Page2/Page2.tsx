import styles from './Page2.module.css'
import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { API_URL } from '../../api/const';
import { TextContainer } from '../../components/TextContainer/TextContainer';

const ROBOT_API_URL = API_URL + "api/robot-hub";
const ROBOT_COMMAND_API_URL = API_URL + "api/robot/command";

export default function Page2() {
    const [robotState, setRobotState] = useState<string>("");

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const formData = new FormData(e.currentTarget);

        const data = {
            commands: formData.get("commands"),
        };

        await fetch(ROBOT_COMMAND_API_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        });

        alert("Wysłano");
    };

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(ROBOT_API_URL)
            .withAutomaticReconnect()
            .build();

        connection.start()
            .then(() => console.log("Connected"))
            .catch(() => console.error("Failed to connect"))

        connection.on("ReceiveRobotState", (message) => {
            setRobotState(message);
            console.log(message);
        });

        return () => {
            connection.stop();
        };
    }, []);

    return (
        <div className={styles.container}>
            <p>Status robota</p>
            <TextContainer title="Ostatnia synchronizacja" text={robotState}></TextContainer>
            
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Komendy:</label>
                    <input name="commands" />
                </div>
                <button type="submit">Wyślij</button>
            </form>
        </div>
    );
}