import styles from './Page2.module.css'
import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { API_URL } from '../../api/const';
import { TextContainer } from '../../components/TextContainer/TextContainer';

const ROBOT_API_URL = API_URL + "api/robot-hub";

export default function Page2() {
    const [robotState, setRobotState] = useState<string>("");

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
        </div>
    );
}