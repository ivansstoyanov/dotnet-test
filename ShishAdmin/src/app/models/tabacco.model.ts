export class Tabacco {
    constructor(data) {
        this.id = data.id;
        this.internalId = data.internalId;
        this.name = data.name;
        this.type = data.type;
        this.picture = data.picture;
        this.power = data.power;
        this.relatedTabaccos = new Array<Tabacco>();
        for (let tb in data.relatedTabaccos) {
            this.relatedTabaccos.push(new Tabacco(tb));
        }
    }
    id: string;
    internalId: string;
    name: string;
    type: string;
    picture: string;
    power: number;
    relatedTabaccos: Tabacco[];
}
