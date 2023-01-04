//@ts-check
import { useCallback, useRef, useState } from 'react';

const isFunction = (setStateAction) => typeof setStateAction === 'function';

const useStateRef = (initialState) => {
    const [state, setState] = useState(initialState);
    const ref = useRef(state);
    const dispatch = useCallback((setStateAction) => {
        ref.current = isFunction(setStateAction) ? setStateAction(ref.current) : setStateAction;
        setState(ref.current);
    }, []);
    return [state, dispatch, ref];
};

export default useStateRef;
