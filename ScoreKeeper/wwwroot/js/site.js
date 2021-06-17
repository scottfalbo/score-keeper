'use strict';


//function scrollToBottom() {
//    var element = document.getElementById("scroll-me-dev");
//    element.scrollTop = element.scrollHeight;
//}

const theElement = document.getElementById('dev');

const scrollToBottom = (node) => {
	node.scrollTop = node.scrollHeight;
}

scrollToBottom(theElement);

