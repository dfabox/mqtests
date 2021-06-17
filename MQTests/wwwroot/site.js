
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
    } catch (error) {
        console.error('Ошибка:', error);
    }
}
