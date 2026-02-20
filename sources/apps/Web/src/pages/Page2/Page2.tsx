import styles from './Page2.module.css'
import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { API_URL } from '../../api/const';

const ROBOT_API_URL = API_URL + "api/robot-hub";

export default function Page2() {
    const [robotState, setRobotState] = useState("")

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
        });

        return () => {
            connection.stop();
        };
    }, []);

    return (
        <div className={styles.container}>
            <p>Page2</p>
            <p>{robotState}</p>
        </div>
    );
}