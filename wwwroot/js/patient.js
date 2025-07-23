let allPatients = [];

document.addEventListener('DOMContentLoaded', () => {
    fetch('/api/patient')
        .then(res => res.json())
        .then(data => {
            allPatients = data;
            renderPatients(allPatients);
        });

    document.getElementById('searchBox').addEventListener('input', e => {
        const keyword = e.target.value.toLowerCase();
        const filtered = allPatients.filter(p =>
            p.name.toLowerCase().includes(keyword) ||
            p.roomNumber.toLowerCase().includes(keyword)
        );
        renderPatients(filtered);
    });
});

function renderPatients(patients) {
    const tbody = document.querySelector('#patientsTable tbody');
    tbody.innerHTML = '';

    patients.forEach(p => {
        const row = document.createElement('tr');
        row.innerHTML = `
        <td><a href="/Patient/VitalSigns/${p.id}">${p.name}</a></td>
        <td>${p.age}</td>
        <td>${p.roomNumber}</td>
    `;
        tbody.appendChild(row);
    });
}


function showVitals(patientId) {
    const patient = allPatients.find(p => p.id === patientId);
    document.getElementById("patientName").textContent = patient.name;
    document.getElementById("vitalsSection").classList.remove('d-none');

    fetch(`/api/patient/${patientId}/vitals`)
        .then(res => res.json())
        .then(vitals => {
            const list = document.getElementById("vitalsList");
            list.innerHTML = '';
            vitals.forEach(v => {
                const item = document.createElement('li');
                item.textContent = `🫀 HR: ${v.heartRate} | BP: ${v.systolicBP}/${v.diastolicBP} | O2: ${v.oxygenSaturation}% @ ${new Date(v.timestamp).toLocaleTimeString()}`;
                list.appendChild(item);
            });
        });
}
