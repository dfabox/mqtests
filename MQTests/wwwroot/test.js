﻿async function doApiTest() {
    startTime = new Date();

    document.getElementById('testResult').innerHTML = "";
    document.getElementById('totalTime').innerHTML = "";

    const reqCount = document.getElementById('reqCount');
    const count = reqCount ? reqCount.value : 10;

    // Получим массивы случайных значений городов и ip
    // Города из базы, а ip могут быть и не из базы
    response = await fetch('/test/rndip?count=' + count.toString(), {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const ipList = await response.json();
    //console.log('ipList:', JSON.stringify(ipList));

    response = await fetch('/test/rndcity?count=' + count.toString(), {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const cityList = await response.json();
    //console.log('cityList:', JSON.stringify(cityList));

    var totalTimeMs = { value: 0.0, count: 0 }

    // По тестовому массиву сформировать запросы поиска
    ipList.Items.forEach(async function (item, index) {
        await doTest('Ip', item, totalTimeMs);
    });

    // По тестовому массиву сформировать запросы поиска
    cityList.Items.forEach(async function (item, index) {
        await doTest('City', item, totalTimeMs);
    });

    endTime = new Date();
    var timeDiff = endTime - startTime; //in ms

    // get seconds 
    var seconds = Math.round(timeDiff);

    document.getElementById('testResult').innerHTML =
        'время отправки запросов: ' + timeDiff.toString();
}

async function doTest(mode, data, totalTimeMs) {
    const url = (mode == 'Ip' ? '/ip/location' : mode == 'City' ? '/city/locations' : 'error')
        + '?text=' + encodeURI(data);
    //console.log(url);

    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const json = await response.json();
        //console.log('Успех:', JSON.stringify(json));

        // Показатели эффективность поиска
        totalTimeMs.value += json.TimeMs;
        totalTimeMs.count += 1;

        let tRounded = Math.round(totalTimeMs.value, -3);
        var perSec = Math.round(1000 * totalTimeMs.count / totalTimeMs.value, -1);

        document.getElementById('totalTime').innerHTML =
            'обработано: ' + totalTimeMs.count.toString() + ', время поиска: ' + tRounded.toString() + ' мс, в секунду ' + perSec.toString();

    } catch (error) {
        console.error('Ошибка:', error);
    }
}