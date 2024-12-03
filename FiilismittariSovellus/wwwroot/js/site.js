

document.addEventListener("DOMContentLoaded", function () {
    // Lisää kuvat nappeihin
    const buttons = document.querySelectorAll(".mood-button");
    buttons.forEach(button => {
        const moodValue = button.getAttribute("data-mood");
        const img = document.createElement("img");
        img.src = `/Pictures/Faces/Emoji_${moodValue}.png`;
        img.alt = `Emoji ${moodValue}`;
        button.appendChild(img);
    });

    // Luo kuvaaja
    const ctx = document.getElementById('moodChart').getContext('2d');
    const moodChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: [], // Lisää päivämäärät tähän
            datasets: [{
                label: 'Fiilismittarin arvo',
                data: [], // Lisää fiilismittarin arvot tähän
                borderColor: 'rgba(75, 192, 192, 1)',
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                fill: true,
            }]
        },
        options: {
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'day'
                    }
                },
                y: {
                    beginAtZero: true,
                    min: 1,
                    max: 7
                }
            }
        }
    });

    // Hae fiilismittarin arvot palvelimelta ja päivitä kuvaaja
    fetch('/api/mooddata')
        .then(response => response.json())
        .then(data => {
            console.log('Fetched data:', JSON.stringify(data, null, 2)); // Lisää tämä lokitus
            moodChart.data.labels = data.map(entry => entry.date);
            moodChart.data.datasets[0].data = data.map(entry => entry.value);
            moodChart.update();
        })
        .catch(error => console.error('Error fetching mood data:', error));
});