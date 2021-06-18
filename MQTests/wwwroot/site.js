
function switchPanels(idShow, idHide) {
    document.getElementById('panel' + idShow).style.display = "block";
    document.getElementById('panel' + idHide).style.display = "none";

    document.getElementById('btn' + idShow).classList.add("selected");
    document.getElementById('btn' + idHide).classList.remove("selected");
}

async function searchBy(id) {
    const search = document.getElementById('input' + id);
    await getGeoData(id, search.value);
}

async function getGeoData(mode, data) {
    // Запрос поиска данных
    //   mode = 'ip' - для поиска по ip, 'city' - для поиска по городу
    //   param - параметр для поиска

    if (!mode || !data) {

        // TODO уточнить в чем ошибка
        alert('Некорректные параметры запроса местоположения.');

        return;
    }

    // Скрыть результаты предыдущего поиска и ошибки
    document.getElementById('searchResult').style.display = "none";
    document.getElementById('searchError').style.display = "none";

    // Запрос
    const url = (mode == 'Ip' ? '/ip/location' : mode == 'City' ? '/city/locations' : 'error')
        + '?text=' + encodeURI(data);
    console.log(url);

    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const json = await response.json();
        console.log('Успех:', JSON.stringify(json));

        if (json.Status == 3) {
            showError(json.Msg);
            alert(json.Msg);
        }
        else {
            const info = getResultInfo(json);
            document.getElementById('searchResult').innerHTML = info;
            document.getElementById('searchResult').style.display = "block";
        }
    } catch (error) {
        console.error('Ошибка:', error);
        showError(error);
    }
}

function showError(error) {
    document.getElementById('searchError').innerHTML = error;
    document.getElementById('searchError').style.display = "block";
}

function addColumn(value) {
    return '<td>' + value + '</td>';
}

function getResultInfo(json) {

    var result = json.Status == 1 ? 'Успешный поиск (' + json.TimeMs.toString() + ' мс )' : 'Данные не найдены';

    if (json.Status == 1 && json.Locations) {
        // Формирование таблицы со списком найденных локаций

        result += '<br>Список:<br>';

        var table = '<table width="100%" class="result-table" cellpadding="5">';
        // Шапка

        table += '<tr class="tr-head">';
        table += addColumn('Страна');
        table += addColumn('Регион');
        table += addColumn('Индекс');
        table += addColumn('Город');
        table += addColumn('Организация');
        table += addColumn('Координаты');
        table += '</tr>';

        json.Locations.forEach(function (item, index) {
            console.log(item);

            table += '<tr>';
            table += addColumn(item.Country);
            table += addColumn(item.Region);
            table += addColumn(item.Postal);
            table += addColumn(item.City);
            table += addColumn(item.Organization);
            table += addColumn('(' + item.Latitude + ', ' + item.Longitude + ')');
            table += '</tr>';
        });

        table += '</table>';

        result += '<br>' + table;
    }

    return result;
}