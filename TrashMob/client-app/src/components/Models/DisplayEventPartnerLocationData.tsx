import { Guid } from "guid-typescript";

class DisplayEventPartnerLocationData {
    eventId: string = Guid.createEmpty().toString();
    partnerId: string = Guid.createEmpty().toString();
    partnerLocationId: string = Guid.createEmpty().toString();
    eventPartnerStatusId: number = 0;
    createdByUserId: string = "";
    partnerName: string = "";
    partnerLocationName: string = "";
    partnerLocationNotes: string = "";
}

export default DisplayEventPartnerLocationData;