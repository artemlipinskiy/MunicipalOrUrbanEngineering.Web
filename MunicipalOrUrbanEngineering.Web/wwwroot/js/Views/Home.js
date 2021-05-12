$(document).ready(async function () {
    $('#loading').addClass('hide');
    $('#erorr-message').hide();
    $('#success-message').hide();

    await ShowBulletinsBoard();
});

function StartLoading() {
    $('#loading').removeClass('hide');
}

function EndLoading() {
    $('#loading').addClass('hide');
}

async function ShowBulletinsBoard() {
    StartLoading();
    $('#BulletinBoardContainer').empty();
    var bulletins = await GetBulletinBoards();
    for (var i = 0; i < bulletins.length; i++) {
        var item = '<div class="col-4">' +
            '<h2>' + bulletins[i].title + '</h2>' +
            '<p>' + bulletins[i].shortDescription + '</p>'
            + ' <p>' + '<button type="button" class="btn btn-secondary" id="'+bulletins[i].id+'" onclick="ShowDetails(id)"> Прочитать </button ></p>'+
        '</div>';
        $('#BulletinBoardContainer').append(item);
    }
    EndLoading();
}

async function GetBulletinBoards() {
    var result = await $.ajax({
        type: 'GET',
        url: '/bulletinboard/GetActual',
        data: {

        }
    });
    return result;
}


async function ShowDetails(id) {
    StartLoading();
    var bulletin = await GetBulletin(id);

    $('#details-title').empty();
    $('#details-author').empty();
    $('#details-text').empty();


    $('#details-title').append(bulletin.title);
    $('#details-author').append('Автор: '+bulletin.creatorFullName);
    $('#details-text').append(bulletin.description);

    $('#BulletinDetailsDialog').modal('show');
    EndLoading();
}
async function GetBulletin(id) {
    var result = await $.ajax({
        type: 'GET',
        url: '/BulletinBoard/Get',
        data: {
            Id:id
        }
    });
    return result;
}

function Cancel()
{
    $('#BulletinDetailsDialog').modal('hide');
}