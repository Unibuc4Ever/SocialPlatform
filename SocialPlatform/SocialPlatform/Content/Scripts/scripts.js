function sendPostLikeForm(postId, el, cnt_likes) {
	$.ajax({
		url: "/Likes/New",
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify({
			PostId: postId,
			type: "like"
		}),
		success: function () {
			var eln = document.getElementById(el);
			eln.innerHTML = (cnt_likes + 1) + " <i class='material-icons btn-liked'>thumb_up</i>";
			eln.setAttribute("onclick", `javascript: sendPostUnLikeForm(${postId}, '${el}', ${cnt_likes + 1})`);
		},
		error: function (jqXHR, exception) {
			alert('Error message. Failed my Like');
		}
	});
}

function sendPostUnLikeForm(postId, el, cnt_likes) {
	$.ajax({
		url: "/Likes/DeletePostLike",
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify({
			PostId: postId
		}),
		success: function () {
			var eln = document.getElementById(el);
			eln.innerHTML = (cnt_likes - 1) + " <i class='material-icons btn-not-liked'>thumb_up</i>";
			eln.setAttribute("onclick", `javascript: sendPostLikeForm(${postId}, '${el}', ${cnt_likes - 1})`);
		},
		error: function (jqXHR, exception) {
			alert('Error message. Failed my Like');
		}
	});
}

function sendCommentLikeForm(commentId, el, cnt_likes) {
	$.ajax({
		url: "/Likes/New",
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify({
			CommentId: commentId,
			type: "like"
		}),
		success: function () {
			var eln = document.getElementById(el);
			eln.innerHTML = (cnt_likes + 1) + " <i class='material-icons btn-liked'>thumb_up</i>";
			eln.setAttribute("onclick", `javascript: sendCommentUnLikeForm(${commentId}, '${el}', ${cnt_likes + 1})`);
		},
		error: function (jqXHR, exception) {
			alert('Error message. Failed my Like');
		}
	});
}

function sendCommentUnLikeForm(commentId, el, cnt_likes) {
	$.ajax({
		url: "/Likes/DeleteCommentLike",
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify({
			CommentId: commentId
		}),
		success: function () {
			var eln = document.getElementById(el);
			eln.innerHTML = (cnt_likes - 1) + " <i class='material-icons btn-not-liked'>thumb_up</i>";
			eln.setAttribute("onclick", `javascript: sendCommentLikeForm(${commentId}, '${el}', ${cnt_likes - 1})`);
		},
		error: function (jqXHR, exception) {
			alert('Error message. Failed my Like');
		}
	});
}


// Used for sending friend requests.
// They require the buttons to have particular IDs.

function sendFriendRequest(otherId) {
	$.ajax({
		url: "/Users/FriendRequest/New/" + otherId,
		type: 'POST',
		success: function () {
			var el = document.getElementById('friend_request_button_' + otherId);
			el.textContent = 'Cancel Friend Request';
			el.setAttribute("onclick", "cancelFriendRequest('" + otherId + "')");
		},
		error: function (jqXHR, exception) {
			alert('Error message. Failed to send request!');
		}
	})
}

function cancelFriendRequest(otherId) {
	$.ajax({
		url: "/Users/FriendRequest/Cancel/" + otherId,
		type: 'POST',
		success: function () {
			var el = document.getElementById('friend_request_button_' + otherId);
			el.textContent = 'Friend Request';
			el.setAttribute("onclick", "sendFriendRequest('" + otherId + "')");
		},
		error: function (jqXHR, exception) {
			alert('Error message. Failed to cancel request!');
		}
	})
}

function acceptFriendRequest(otherId) {
	$.ajax({
		url: "/Users/FriendRequest/Accept/" + otherId,
		type: "POST",
		success: function () {
			var el = document.getElementById('friend_request_button_' + otherId);
			el.textContent = 'Unfriend';
			el.setAttribute("onclick", "unFriend('" + otherId + "')");
		},
		error: function (jqXHR, exception) {
			alert('Error message. Failed to accept!');
		}
	})
}

function unFriend(otherId) {
	$.ajax({
		url: "/Users/Friends/Unfriend/" + otherId,
		type: 'POST',
		success: function () {
			var el = document.getElementById('friend_request_button_' + otherId);
			el.textContent = 'Friend Request';
			el.setAttribute("onclick", "sendFriendRequest('" + otherId + "')");
		},
		error: function (jqXHR, exception) {
			alert('Error message. Failed to send unfriend request!');
		}
	})
}

