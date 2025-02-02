const express = require('express');
const app = express();
const path = require('path');

// Serve static files from the 'public' folder
app.use(express.static(path.join(__dirname, 'public')));

// Version endpoint
app.get('/version.txt', (req, res) => {
    res.sendFile(path.join(__dirname, 'public', 'version.txt'));
});

// Windows patch route
app.get('/patch_windows.zip', (req, res) => {
    res.sendFile(path.join(__dirname, 'public', 'patch_windows.zip'));
});

// macOS patch route
app.get('/patch_mac.zip', (req, res) => {
    res.sendFile(path.join(__dirname, 'public', 'patch_mac.zip'));
});

// Start the server
app.listen(3000, () => {
    console.log('Patch server running on http://localhost:3000');
});
