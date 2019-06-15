export interface Message {
    id: number;
    senderId: number;
    senderNickname: string;
    senderImageUrl: string;
    recipientId: number;
    recipientNickname: string;
    recipientImageUrl: string;
    content: string;
    isRead: boolean;
    readDate: Date;
    sentDate: Date;
}
