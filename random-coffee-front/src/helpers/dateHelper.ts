export const formatDate = (dateString: string) : string => {
	const parts = dateString.split('T');
	return parts[0];
}
