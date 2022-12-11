var urlParams = new URLSearchParams(window.location.search);
var pageSize = 4;
var initialWallID = -1;

function getParametersString(lastWallID) {
    return `?query=${urlParams.get('query')}` +
           `&cnt=${pageSize}` +
        `&lastWallID=${lastWallID}`;
}

var commonFirstParameters = getParametersString(initialWallID);

var nextGroupsURL = `/Search/PagedGroups` + commonFirstParameters;
var nextUsersURL = `/Search/PagedUsers` + commonFirstParameters;
var nextPostsURL = `/Search/PagedPosts` + commonFirstParameters;

// Code for Groups
function updateNextGroupsURL(groups_resp) {
    nextGroupsURL = `/Search/PagedGroups` + getParametersString(groups_resp.lastWallID);
}

function createGroupElement(groupInfo) {
    var element = document.createElement('div');
    element.className = 'demo-card-event mdl-card mdl-shadow--2dp';
    htmlText = `
        <div class="mdl-card__title mdl-card--expand">
        <h4>
        Group Name:<br>
            ${groupInfo.Name}<br>
                Members:<br>
                    ${groupInfo.MemberCount}
            </h4>
        </div>
            <div class="mdl-card__actions mdl-card--border">
                <a class="mdl-button mdl-button--colored mdl-js-button mdl-js-ripple-effect"
                    href="/Group/Show/${groupInfo.GroupID}">
                    View Group
            </a>
                <div class="mdl-layout-spacer"></div>
                <i class="material-icons">event</i>
            </div>`;
    element.innerHTML = htmlText;
    return element;
}

var infScroll = undefined;

function initScrolls() {
    infScroll = new InfiniteScroll(document.querySelector('#group-container'), {
        path: function () {
            return nextGroupsURL;
        },

        responseType: 'json',
        status: '.scroll-status',
        history: false,
        loadOnScroll: true
    });

    // use element to turn HTML string into elements
    var proxyElem = document.createElement('div');

    infScroll.on('load', function (response) {
        // parse response into JSON data
        console.log("Load from inf-Scroll, data: ", response);

        if (response.groups.length < 1) {
            document.querySelector('#tab2-panel > button').style.display = "none";
        }
        //var data = JSON.parse(response);
        //// compile data into HTML
        //var itemsHTML = data.map(getItemHTML).join('');
        //// convert HTML string into elements
        //proxyElem.innerHTML = itemsHTML;
        //// append item elements
        //var items = proxyElem.querySelectorAll('.photo-item');
        infScroll.appendItems(response.groups.map(createGroupElement));
        updateNextGroupsURL(response);
    });

    // load initial page
    infScroll.loadNextPage();
}

document.addEventListener("DOMContentLoaded", function (event) {
    initScrolls();
});
