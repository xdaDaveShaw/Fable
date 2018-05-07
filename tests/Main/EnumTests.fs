module Fable.Tests.Enum

open FSharp.Core.LanguagePrimitives
open Util.Testing

type Fruits =
| Apple = 1
| Banana = 2
| Coconut = 4

type Vegetables =
| Tomato of string
| Lettuce of string

let myRootValue =
    match 5 with
    // mutable is used to prevent immutable binding optimization
    | 0 -> let mutable r = 4 in r + 4
    | 5 -> let mutable r = 3 in r + 7
    | _ -> -1

let tests =
  testList "Enum" [
    testCase "Enum.HasFlag works" <| fun () ->
        let value = Fruits.Apple ||| Fruits.Banana
        equal true (value.HasFlag Fruits.Apple)
        equal true (value.HasFlag Fruits.Banana)
        equal false (value.HasFlag Fruits.Coconut)

    testCase "Enum operator = works" <| fun () ->
        let a = Fruits.Apple
        let b = Fruits.Banana

        Fruits.Apple = Fruits.Apple |> equal true
        a = Fruits.Apple |> equal true
        Fruits.Apple = a |> equal true
        a = a |> equal true

        Fruits.Apple = Fruits.Banana |> equal false
        a = Fruits.Banana |> equal false
        Fruits.Banana = a |> equal false
        a = b |> equal false

    testCase "Enum operator <> works" <| fun () ->
        let a = Fruits.Apple
        let b = Fruits.Banana

        Fruits.Apple <> Fruits.Apple |> equal false
        a <> Fruits.Apple |> equal false
        Fruits.Apple <> a |> equal false
        a <> a |> equal false

        Fruits.Apple <> Fruits.Banana |> equal true
        a <> Fruits.Banana |> equal true
        Fruits.Banana <> a |> equal true
        a <> b |> equal true

    testCase "Enum operator < works" <| fun () ->
        let a = Fruits.Apple
        let b = Fruits.Banana

        Fruits.Apple < Fruits.Apple |> equal false
        a < Fruits.Apple |> equal false
        Fruits.Apple < a |> equal false
        a < a |> equal false

        Fruits.Apple < Fruits.Banana |> equal true
        a < Fruits.Banana |> equal true
        Fruits.Banana < a |> equal false
        a < b |> equal true

    testCase "Enum operator <= works" <| fun () ->
        let a = Fruits.Apple
        let b = Fruits.Banana

        Fruits.Apple <= Fruits.Apple |> equal true
        a <= Fruits.Apple |> equal true
        Fruits.Apple <= a |> equal true
        a <= a |> equal true

        Fruits.Apple <= Fruits.Banana |> equal true
        a <= Fruits.Banana |> equal true
        Fruits.Banana <= a |> equal false
        a <= b |> equal true

    testCase "Enum operator > works" <| fun () ->
        let a = Fruits.Apple
        let b = Fruits.Banana

        Fruits.Apple > Fruits.Apple |> equal false
        a > Fruits.Apple |> equal false
        Fruits.Apple > a |> equal false
        a > a |> equal false

        Fruits.Apple > Fruits.Banana |> equal false
        a > Fruits.Banana |> equal false
        Fruits.Banana > a |> equal true
        a > b |> equal false

    testCase "Enum operator >= works" <| fun () ->
        let a = Fruits.Apple
        let b = Fruits.Banana

        Fruits.Apple >= Fruits.Apple |> equal true
        a >= Fruits.Apple |> equal true
        Fruits.Apple >= a |> equal true
        a >= a |> equal true

        Fruits.Apple >= Fruits.Banana |> equal false
        a >= Fruits.Banana |> equal false
        Fruits.Banana >= a |> equal true
        a >= b |> equal false

    testCase "EnumOfValue works" <| fun () ->
        EnumOfValue 1 |> equal Fruits.Apple
        EnumOfValue 2 |> equal Fruits.Banana
        EnumOfValue 4 |> equal Fruits.Coconut

    testCase "Enum operator enum works" <| fun () ->
        enum 1 |> equal Fruits.Apple
        enum 2 |> equal Fruits.Banana
        enum 4 |> equal Fruits.Coconut

    testCase "Pattern matching can be nested within a switch statement" <| fun () -> // See #483
        let fruit = Fruits.Apple
        let veggie = Tomato("kumato")
        match fruit with
        | Fruits.Apple ->
            match veggie with
            | Tomato kind -> kind.Replace("to","")
            | _ -> "foo"
        | Fruits.Banana
        | Fruits.Coconut ->
            match veggie with
            | Lettuce kind -> kind
            | _ -> "bar"
        | _ -> "invalid choice"
        |> equal "kuma"

    testCase "Non-scoped (in JS) variables with same name can be used" <| fun () -> // See #700
        equal 10 myRootValue
  ]