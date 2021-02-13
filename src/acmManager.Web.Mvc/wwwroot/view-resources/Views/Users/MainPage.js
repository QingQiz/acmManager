let chartColors = {
    blue: "rgb(54, 162, 235)",
    green: "rgb(75, 192, 192)",
    grey: "rgb(201, 203, 207)",
    orange: "rgb(255, 159, 64)",
    purple: "rgb(153, 102, 255)",
    red: "rgb(255, 99, 132)",
    yellow: "rgb(255, 205, 86)",
};

$(function () {
    let labels = $('input[name="problem-types"]').val().split(',').map(s => s.split(':'));
    
    let color = Chart.helpers.color;
    
    let radarChart = new Chart($('#radar-chart')[0], {
        type: 'radar',
        data: {
            labels: labels.map(l => l[0]),
            datasets: [{
                label: "题目数量",
                data: labels.map(l => parseInt(l[1])),
                backgroundColor: color(chartColors.red).alpha(0.2).rgbString(),
                borderColor: color(chartColors.red).alpha(0.7).rgbString()
            }]
        },
        options: {
            responsive: false,
            aspectRatio: 1,
            title: {
                display: true,
                text: '题目类型统计',
                fontSize: 15
            },
            scale: {
                ticks: {
                    stepSize: 1
                }
            },
            tooltips: {
                callbacks: {
                    title: (item, data) => data.labels[item[0].index]
                }
            }
        }
    });
});