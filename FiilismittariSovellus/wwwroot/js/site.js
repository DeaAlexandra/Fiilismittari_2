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

    // Funktio, joka luo kaikki kuukauden päivämäärät
    function getMonthDates(year, month) {
        const dates = [];
        const date = new Date(year, month, 1);
        while (date.getMonth() === month) {
            dates.push(new Date(date));
            date.setDate(date.getDate() + 1);
        }
        return dates;
    }

    // Funktio, joka luo kaikki viikon päivämäärät
    function getWeekDates(year, month, day) {
        const dates = [];
        const date = new Date(year, month, day);
        const dayOfWeek = date.getDay();
        const startOfWeek = new Date(date);
        startOfWeek.setDate(date.getDate() - dayOfWeek + 1); // Aloita maanantaista
        for (let i = 0; i < 7; i++) {
            const weekDate = new Date(startOfWeek);
            weekDate.setDate(startOfWeek.getDate() + i);
            dates.push(weekDate);
        }
        return dates;
    }

    // Luo kaikki kuukauden päivämäärät
    const today = new Date();
    const year = today.getFullYear();
    const month = today.getMonth();
    const monthDates = getMonthDates(year, month);

    // Luo kuvaaja
    const ctx = document.getElementById('moodChart').getContext('2d');
    const moodChart = new Chart(ctx, {
        type: 'bar', // Muutetaan tyyppi pylväsdiagrammiksi
        data: {
            labels: monthDates.map(date => date.toISOString().split('T')[0]), // Lisää päivämäärät tähän
            datasets: [{
                label: 'Fiilismittarin arvo',
                data: monthDates.map(() => null), // Alusta arvot null-arvoilla
                backgroundColor: monthDates.map(() => 'rgba(200, 200, 200, 0.2)'), // Alusta värit harmaalla
                borderColor: monthDates.map(() => 'rgba(255, 255, 255, 0.8)'), // Vaaleat reunukset
                borderWidth: 5, // Reunuksen leveys
                borderRadius: 15, // Pyöristää pylväiden yläreunat
                barThickness: 30, // Säätää pylväiden leveyttä
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: true,
                    position: 'top', // Muuta legendan sijaintia
                },
                tooltip: {
                    enabled: true,
                    mode: 'index',
                    intersect: false,
                }
            },
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'day'
                    },
                    title: {
                        display: true,
                        text: 'Päivämäärä', // Lisää x-akselin otsikko
                        color: 'rgba(227, 227, 227, 0.8)', // Muuta x-akselin otsikon väriä
                        font: {
                            family: 'Arial',
                            size: 14,
                            weight: 'bold',
                        }
                    },
                    ticks: {
                        color: 'rgba(227, 227, 227, 0.8)', // Muuta x-akselin tekstin väriä
                    }
                },
                y: {
                    beginAtZero: true,
                    min: 1,
                    max: 7,
                    title: {
                        display: true,
                        text: 'Arvo', // Lisää y-akselin otsikko
                        color: 'rgba(227, 227, 227, 0.8)', // Muuta y-akselin otsikon väriä
                        font: {
                            family: 'Arial',
                            size: 14,
                            weight: 'bold',
                        }
                    },
                    ticks: {
                        color: 'rgba(227, 227, 227, 0.8)', // Muuta y-akselin tekstin väriä
                    }
                }
            }
        }
    });

    // Hae fiilismittarin arvot palvelimelta ja päivitä kuvaaja
    fetch('/api/mooddata')
        .then(response => response.json())
        .then(data => {
            console.log('Fetched data:', JSON.stringify(data, null, 2)); // Lisää tämä lokitus

            // Päivitä arvot ja värit tietokannasta haettujen arvojen perusteella
            moodChart.data.datasets[0].data = monthDates.map(date => {
                const entry = data.find(entry => {
                    const entryDate = new Date(entry.date).toISOString().split('T')[0];
                    return entryDate === date.toISOString().split('T')[0];
                });
                return entry ? entry.value : null;
            });
            moodChart.data.datasets[0].backgroundColor = monthDates.map(date => {
                const entry = data.find(entry => {
                    const entryDate = new Date(entry.date).toISOString().split('T')[0];
                    return entryDate === date.toISOString().split('T')[0];
                });
                if (entry) {
                    if (entry.value == 1) return 'rgba(157, 4, 0, 0.8)';
                    if (entry.value == 2) return 'rgba(219, 28, 4, 0.8)'; // Punainen, jos arvo on 2
                    if (entry.value == 3) return 'rgba(223, 99, 13, 0.8)';
                    if (entry.value == 4) return 'rgba(242, 174, 5, 0.8)';
                    if (entry.value == 5) return 'rgba(162, 212, 38, 0.8)';
                    if (entry.value == 6) return 'rgba(113, 153, 9, 0.8)';
                    if (entry.value == 7) return 'rgb(89, 120, 7)';
                }
                return 'rgba(200, 200, 200, 0.2)'; // Harmaa, jos ei tietoa
            });
            moodChart.data.datasets[0].borderColor = monthDates.map(date => {
                const entry = data.find(entry => {
                    const entryDate = new Date(entry.date).toISOString().split('T')[0];
                    return entryDate === date.toISOString().split('T')[0];
                });
                if (entry) {
                    if (entry.value == 1) return 'rgba(227, 227, 227, 0.8)';
                    if (entry.value == 2) return 'rgba(227, 227, 227, 0.8)'; // Punainen, jos arvo on 2
                    if (entry.value == 3) return 'rgba(227, 227, 227, 0.8)';
                    if (entry.value == 4) return 'rgba(227, 227, 227, 0.8)';
                    if (entry.value == 5) return 'rgba(227, 227, 227, 0.8)';
                    if (entry.value == 6) return 'rgba(227, 227, 227, 0.8)';
                    if (entry.value == 7) return 'rgba(227, 227, 227, 0.8)';
                }
                return 'rgba(255, 255, 255, 0.8)'; // Vaalea reunus, jos ei tietoa
            });
            
            moodChart.update();

            // Funktio päivittämään kuvaajan näkymä
            function updateChartView(viewType) {
                let dates;
                if (viewType === 'week') {
                    const sliderValue = parseInt(document.getElementById('dateRange').value, 10);
                    dates = getWeekDates(year, month, sliderValue);
                } else {
                    dates = monthDates;
                }

                // Päivitä arvot ja värit tietokannasta haettujen arvojen perusteella
                moodChart.data.labels = dates.map(date => date.toISOString().split('T')[0]);
                moodChart.data.datasets[0].data = dates.map(date => {
                    const entry = data.find(entry => {
                        const entryDate = new Date(entry.date).toISOString().split('T')[0];
                        return entryDate === date.toISOString().split('T')[0];
                    });
                    return entry ? entry.value : null;
                });
                moodChart.data.datasets[0].backgroundColor = dates.map(date => {
                    const entry = data.find(entry => {
                        const entryDate = new Date(entry.date).toISOString().split('T')[0];
                        return entryDate === date.toISOString().split('T')[0];
                    });
                    if (entry) {
                        if (entry.value == 1) return 'rgba(157, 4, 0, 0.8)';
                        if (entry.value == 2) return 'rgba(219, 28, 4, 0.8)'; // Punainen, jos arvo on 2
                        if (entry.value == 3) return 'rgba(223, 99, 13, 0.8)';
                        if (entry.value == 4) return 'rgba(242, 174, 5, 0.8)';
                        if (entry.value == 5) return 'rgba(162, 212, 38, 0.8)';
                        if (entry.value == 6) return 'rgba(113, 153, 9, 0.8)';
                        if (entry.value == 7) return 'rgb(89, 120, 7)';
                    }
                    return 'rgba(200, 200, 200, 0.2)'; // Harmaa, jos ei tietoa
                });
                moodChart.data.datasets[0].borderColor = dates.map(date => {
                    const entry = data.find(entry => {
                        const entryDate = new Date(entry.date).toISOString().split('T')[0];
                        return entryDate === date.toISOString().split('T')[0];
                    });
                    if (entry) {
                        if (entry.value == 1) return 'rgba(227, 227, 227, 0.8)';
                        if (entry.value == 2) return 'rgba(227, 227, 227, 0.8)'; // Punainen, jos arvo on 2
                        if (entry.value == 3) return 'rgba(227, 227, 227, 0.8)';
                        if (entry.value == 4) return 'rgba(227, 227, 227, 0.8)';
                        if (entry.value == 5) return 'rgba(227, 227, 227, 0.8)';
                        if (entry.value == 6) return 'rgba(227, 227, 227, 0.8)';
                        if (entry.value == 7) return 'rgba(227, 227, 227, 0.8)';
                    }
                    return 'rgba(227, 227, 227, 0.8)'; // Vaalea reunus, jos ei tietoa
                });

                moodChart.update();
            }

            // Lisää tapahtumankuuntelijat painikkeille ja sliderille
            document.getElementById('weekView').addEventListener('click', () => updateChartView('week'));
            document.getElementById('monthView').addEventListener('click', () => updateChartView('month'));
            document.getElementById('dateRange').addEventListener('input', () => updateChartView('week'));
        })
        .catch(error => console.error('Error fetching mood data:', error));
});