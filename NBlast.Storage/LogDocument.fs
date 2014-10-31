namespace NBlast.Storage

type StorageType = 
    | Analysed = 1 
    | Stored = 2
    | Both = 3 

type Field<'T> = {
    Value : 'T
    Name: string
    StorageType: StorageType
}

type LogDocument = {
    message: Field<string>
    logger: Field<string>
    level: Field<string>
}

