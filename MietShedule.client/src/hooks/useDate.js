import { useState, useCallback } from "react";

export default function useDate(defaultValue=new Date()){
    const [date, setDate] = useState(defaultValue.toISOString().split('T')[0])
    const addDay = useCallback(() => {
        let newdate = new Date(date)
        newdate.setDate(newdate.getDate() + 1)
        setDate(newdate.toISOString().split('T')[0])
    }, [date])


    const minusDay = useCallback(() => {
        let newdate = new Date(date)
        newdate.setDate(newdate.getDate() - 1)
        setDate(newdate.toISOString().split('T')[0])
    }, [date])

    return {value:date, setDate, minusDay, addDay}
}