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
