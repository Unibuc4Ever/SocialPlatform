function sendPostLikeForm(postId) {
	$.ajax({
		url: "/Likes/New",
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify({
			PostId: postId,
			type: "like"
		}),
		success: function () {
			location.reload();
		},
		error: function (jqXHR, exception) {
			alert('Error message. Failed my Like');
		}
	});
}

function sendCommentLikeForm(commentId) {
	$.ajax({
		url: "/Likes/New",
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify({
			CommentId: commentId,
			type: "like"
		}),
		success: function () {
			location.reload();
		},
		error: function (jqXHR, exception) {
			alert('Error message. Failed my Like');
		}
	});
}

function sendUnLikeForm(likeId) {
	$.ajax({
		url: "/Likes/Delete",
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify({
			LikeId: likeId
		}),
		success: function () {
			location.reload();
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
			var el = document.getElementById('friend_request_button');
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
			var el = document.getElementById('friend_request_button');
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
			var el = document.getElementById('friend_request_button');
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
			var el = document.getElementById('friend_request_button');
			el.textContent = 'Friend Request';
			el.setAttribute("onclick", "sendFriendRequest('" + otherId + "')");
		},
		error: function (jqXHR, exception) {
			alert('Error message. Failed to send unfriend request!');
		}
	})
}

