const express = require('express');
const { exec } = require('child_process');

const app = express();
app.use(express.urlencoded({ extended: true }));

app.get('/', (req, res) => {
    res.send(`
        <h1>Ping a Website</h1>
        <form method="POST" action="/ping">
            <label for="host">Enter a hostname or IP address:</label>
            <input type="text" id="host" name="host" placeholder="e.g., google.com">
            <button type="submit">Ping</button>
        </form>
    `);
});

const { spawn } = require("child_process");
const express = require("express");
const app = express();

app.use(express.json()); // Middleware to parse JSON requests

app.post("/ping", (req, res) => {
    const host = req.body.host;

    // Validate host to prevent injection
    if (!/^[a-zA-Z0-9.-]+$/.test(host)) {
        return res.status(400).send("Invalid host");
    }

    // Use spawn instead of exec for security
    const ping = spawn("ping", ["-c", "4", host]);

    let output = "";
    ping.stdout.on("data", (data) => {
        output += data.toString();
    });

    ping.stderr.on("data", (data) => {
        output += data.toString();
    });

    ping.on("close", (code) => {
        res.send(`<pre>${output}</pre>`);
    });
});

app.listen(3000, () => {
    console.log('ping app running on http://localhost:3000');
});