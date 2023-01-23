export const getRandomImg = () : string => {
	const names = ['avatar.jpg',  'picture.jpg', 'picture1.jpg', 'picture2.jpg'];
	const randomIndex = getRandomNumber(0, names.length - 1);
	return names[randomIndex];
}

const getRandomNumber = (min: number, max: number): number => {
	const difference = max - min;
	let rand = Math.random();
	rand = Math.floor(rand * difference);
	rand = rand + min;
	return rand;
}
